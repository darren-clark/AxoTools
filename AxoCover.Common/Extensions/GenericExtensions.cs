﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml.Serialization;

namespace AxoCover.Common.Extensions
{
  public static class GenericExtensions
  {
    public static IEnumerable<Type> FindImplementers<T>(this Assembly assembly)
    {
      return assembly.GetExportedTypes()
        .Where(p => p.GetInterfaces().Any(q => q == typeof(T)));
    }

    public static string ToXml<T>(this T value)
    {
      var serializer = new XmlSerializer(typeof(T));
      using (var writer = new StringWriter())
      {
        serializer.Serialize(writer, value);
        writer.Flush();
        return writer.ToString();
      }
    }

    public static string GetDescription(this Exception exception)
    {
      var text = string.Empty;
      while (exception != null)
      {
        text += exception.GetType().Name + ": " + exception.Message + "\r\n";
        text += exception.StackTrace + "\r\n\r\n";
        exception = exception.InnerException;
      }
      return text;
    }

    public static void Exit()
    {
      new Thread(() =>
      {
        Thread.Sleep(500);
        Environment.Exit(0);
      }).Start();
    }

    public static string ToAbsolutePath(this string path)
    {
      if (!Path.IsPathRooted(path))
      {
        path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), path);
      }
      return path;
    }
  }
}