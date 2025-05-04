namespace AuraChat.Templates;

public static class EmailTemplates
{
    public static string GetForgottenPasswordEmailBody(string redirectionLink)
    {
        return @"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Password Reset Instructions</title>
                <style>
                    body {
                        font-family: Arial, sans-serif;
                        background-color: #f4f4f4;
                        margin: 0;
                        padding: 0;
                        color: #333;
                    }
                    .container {
                        max-width: 600px;
                        margin: 20px auto;
                        padding: 20px;
                        background-color: #fff;
                        border-radius: 5px;
                        box-shadow: 0 2px 5px rgba(0,0,0,0.1);
                    }
                    h1 {
                        color: #336699;
                    }
                    p {
                        margin-bottom: 16px;
                        line-height: 1.5;
                    }
                    .reset-link {
                        display: inline-block;
                        padding: 10px 20px;
                        background-color: #0078d7;
                        color: white;
                        text-decoration: none;
                        border-radius: 5px;
                        margin-top: 20px;
                    }
                    .reset-link:hover {
                        background-color: #0056b3;
                    }
                    .warning {
                        color: #8a6d3b;
                        background-color: #fef2d4;
                        border: 1px solid #faebcc;
                        padding: 10px;
                        border-radius: 4px;
                        margin-bottom: 16px;
                    }
                </style>
            </head>
            <body>
                <div class=""container"">
                    <h1>Password Reset Instructions</h1>
                    <p>Dear User,</p>
                    <p>A password reset has been requested for your account. To reset your password, please click the link below:</p>
                    <p class=""text-center"">
                        <a href=""" + redirectionLink + @""" class=""reset-link"" style=""color: white; text-decoration: none;"">Reset Your Password</a>
                    </p>
                    <p>This link will expire in 1 hour. If you did not request a password reset, please ignore this email.</p>
                    <p class=""warning"">
                        <strong>Important Security Note:</strong> For your security, do not share this link with anyone.
                    </p>
                    <p>If you have any trouble resetting your password, please contact our support team.</p>
                    <p>Sincerely,<br><b>AuraChat.<b></p>
                </div>
            </body>
            </html>";
    }
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
