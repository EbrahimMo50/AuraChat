namespace AuraChat.Templates;

public static class EmailTemplates
{
    public static string GetOtpEmailBody(string otp)
    {
        return @"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Your OTP</title>
            <style>
                body {
                    font-family: sans-serif;
                    line-height: 1.6;
                    margin: 0;
                    padding: 20px;
                    background-color: #f4f4f4;
                }
                .container {
                    max-width: 600px;
                    margin: 0 auto;
                    background-color: #ffffff;
                    padding: 30px;
                    border-radius: 8px;
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                }
                .otp-code {
                    font-size: 2em;
                    font-weight: bold;
                    color: #333;
                    background-color: #e9ecef;
                    padding: 10px 20px;
                    border-radius: 5px;
                    display: inline-block;
                    margin: 20px 0;
                }
                .disclaimer {
                    font-size: 0.8em;
                    color: #777;
                    margin-top: 30px;
                }
            </style>
        </head>
        <body>
            <div class=""container"">
                <h2>Your One-Time Password (OTP)</h2>
                <p>Please use the following OTP to complete your verification:</p>
                <div class=""otp-code"">" + otp + @"</div>
                <p>This OTP is valid for a limited time. Do not share it with anyone.</p>
                <p class=""disclaimer"">If you did not request this OTP, please ignore this email.</p>
            </div>
        </body>
        </html>";
    }
}
