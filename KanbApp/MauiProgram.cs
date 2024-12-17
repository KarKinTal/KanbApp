﻿using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using KanbApp.Pages;
using KanbApp.ViewModels;
using KanbApp.Services;

namespace KanbApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Kanit-Black.ttf", "KanitBlack");
                    fonts.AddFont("Kanit-BlackItalic.ttf", "KanitBlackItalic");
                    fonts.AddFont("Kanit-Bold.ttf", "KanitBold");
                    fonts.AddFont("Kanit-BoldItalic.ttf", "KanitBoldItalic");
                    fonts.AddFont("Kanit-ExtraBold.ttf", "KanitExtraBold");
                    fonts.AddFont("Kanit-ExtraBoldItalic.ttf", "KanitExtraBoldItalic");
                    fonts.AddFont("Kanit-ExtraLight.ttf", "KanitExtraLight");
                    fonts.AddFont("Kanit-ExtraLightItalic.ttf", "KanitExtraLightItalic");
                    fonts.AddFont("Kanit-Italic.ttf", "KanitItalic");
                    fonts.AddFont("Kanit-Light.ttf", "KanitLight");
                    fonts.AddFont("Kanit-LightItalic.ttf", "KanitLightItalic");
                    fonts.AddFont("Kanit-Medium.ttf", "KanitMedium");
                    fonts.AddFont("Kanit-MediumItalic.ttf", "KanitMediumItalic");
                    fonts.AddFont("Kanit-Regular.ttf", "KanitRegular");
                    fonts.AddFont("KanitSsemiBold.ttf", "KanitSemiBold");
                    fonts.AddFont("Kanit-SemiBoldItalic.ttf", "KanitSemiBoldItalic");
                    fonts.AddFont("Kanit-Thin.ttf", "KanitThin");
                    fonts.AddFont("Kanit-ThinItalic.ttf", "KanitThinItalic");
                    fonts.AddFont("EduAUVICWANTPRE-Bold.ttf", "EduBold");
                    fonts.AddFont("EduAUVICWANTPRE-Medium.ttf", "EduMedium");
                    fonts.AddFont("EduAUVICWANTPRE-Regular.ttf", "EduRegular");
                    fonts.AddFont("EduAUVICWANTPRE-SemiBold.ttf", "EduSemiBold");
                });
            builder.Services.AddSingleton<IAuthService, AuthService>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<CheckingLoginPage>();

            builder.Services.AddTransient<TablePage>();
            builder.Services.AddTransient<TableViewModel>();

            builder.Services.AddTransient<TableCreatePage>();
            builder.Services.AddTransient<TableCreateViewModel>();

            builder.Services.AddTransient<TableEditPage>();
            builder.Services.AddTransient<TableEditViewModel>();

            builder.Services.AddTransient<UserProfilePage>();
            builder.Services.AddTransient<UserProfileViewModel>();

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LoginViewModel>();

            return builder.Build();
        }
    }
}