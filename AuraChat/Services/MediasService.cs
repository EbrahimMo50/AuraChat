using AuraChat.Configurations;
using AuraChat.Entities;
using AuraChat.Exceptions;
using AuraChat.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace AuraChat.Services;
public class MediaService(IStringLocalizer<MediaService> stringLocalizer, IOptions<UploadSettings> uploadSettingsOptions)
{
    /// <summary>
    /// 
    /// Saves media locally based on 6 segments                                     <br/>
    /// SenderId_RecieverType_RecieverIdentifer_MediaType_Guid_FileName             <br/>
    /// To be called with sender reciever and reciever type encapsuled in the model
    /// 
    /// </summary>
    /// <param name="file"></param>
    /// <param name="reciecerIdentifier"></param>
    /// <returns>composite string with the notion mentioned</returns>
    /// <exception cref="BadRequestException"></exception>
    /// <exception cref="UnsupportedMediaTypeException"></exception>
    public async Task<string> SaveMedia(IFormFile file, MediaDetailsModel mediaDetails)
    {

        var uploadSettings = uploadSettingsOptions.Value;

        if (file.Length == 0)
            throw new BadRequestException(stringLocalizer["Empty file uploaded."]);

        if (file.Length / Math.Pow(1024, 2) > uploadSettings.MaxFileSizeMB)
            throw new UnsupportedMediaTypeException("File is too large maximum limit is 50 MB");

        var errorMessage = stringLocalizer["media type not allowed on server allowed types: "] + string.Join(", ", uploadSettings.AllowedTypes);

        if (!uploadSettings.AllowedTypes.Any(x => x == file.ContentType))
            throw new UnsupportedMediaTypeException(errorMessage);

        Directory.CreateDirectory("./Uploads");

        foreach (var subType in uploadSettings.AllowedTypes.Select(x => x.Split("/").First()).Distinct())
        {
            Directory.CreateDirectory($"./Uploads/{subType}");
        }

        var contentType = file.ContentType.Split("/").First();
        var path = $"./Uploads/{contentType}";
        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName!.Trim('"');

        fileName = $"{mediaDetails.SenderId}_{mediaDetails.RecievrType}_{mediaDetails.RecieverId}_{contentType}_{Guid.NewGuid()}_{fileName}";

        var filePath = Path.Combine(path, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return fileName;
        
    }
    public async Task<IEnumerable<string>> SaveMedia(List<IFormFile> files, MediaDetailsModel mediaDetails)
    {
        IEnumerable<string> listOfMediaPaths = [];
        foreach (var file in files)
        {
            listOfMediaPaths.Append(await SaveMedia(file, mediaDetails));
        }
        return listOfMediaPaths;
    }

    /// <summary>
    /// transforms composite media descriper to structrued model
    /// </summary>
    /// <param name="compositeMediaDescriper"></param>
    /// <returns>details of the media in modular manner</returns>
    public static MediaDetailsModel GetMediaDetails(string compositeMediaDescriper)
    {
        var segments = compositeMediaDescriper.Split('_');
        MediaDetailsModel model = new(
            Convert.ToInt32(segments[0]),
            Enum.Parse<RecieverType>(segments[2]),
            Convert.ToInt32(segments[1]),
            Enum.Parse<MediaType>(segments[3]),
            segments[4],
            segments[5]
            );
        return model;
    }
}