using CustomConfiguration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomConfiguration
{
    public static class MyConfigurationBuilderExstension
    {
        public static IConfigurationBuilder AddCustomConfiguration(this IConfigurationBuilder builder)
        {
            builder.Add(new MyConfigurationSource());
            return builder;
        }
    }
}


