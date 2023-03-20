using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace primer_obravanja_partnerjev {
    public static class Build {
        public static string Configuration {
            get {
                var configuration = "Release";
#if DEBUG
                configuration = "Debug";
#endif
                return configuration;
            }
        }

        public static string Framework {
            get {
                var framework = RuntimeInformation.FrameworkDescription;
                if (framework.Contains(".NET Core")) {
                    return "Core";
                } else if (framework.Contains(".NET Framework")) {
                    return "Framework";
                } else {
                    string error = $"Framework {framework} is not supported.";
                    throw new NotSupportedException(error);
                }
            }
        }

        public static string ProjectPath {
            get {
                var currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                if (currentDirectory.Contains(@"\bin\Debug") || currentDirectory.Contains(@"\bin\Release")) {
                    switch (Framework) {
                        case "Core":
                            return Path.GetFullPath(Path.Combine(currentDirectory, @"..\..\.."));
                        case "Framework":
                            return Path.GetFullPath(Path.Combine(currentDirectory, @"..\.."));
                        default:
                            return Path.GetFullPath(currentDirectory);
                    }
                } else {
                    return Path.GetFullPath(currentDirectory);
                }
            }
        }

        public static string SolutionPath {
            get {
                return Path.GetFullPath(Path.Combine(ProjectPath, ".."));
            }
        }
    }
}
