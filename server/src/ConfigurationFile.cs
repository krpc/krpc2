using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using BepInEx;
using BepInEx.Configuration;
using KRPC;
using KRPC2.Utils;
using KRPC.Server;

namespace KRPC2
{
    sealed class ConfigurationFile
    {
        static ConfigurationFile instance;

        public static ConfigurationFile Instance
        {
            get
            {
                if (instance == null)
                    instance = new ConfigurationFile(Configuration.Instance);
                return instance;
            }
        }

        public Configuration Configuration { get; private set; }

        ConfigFile configFile;

        ConfigEntry<string> serverIds;
        Dictionary<Guid, List<ConfigEntryBase>> servers = new Dictionary<Guid, List<ConfigEntryBase>>();
        ConfigEntry<bool> mainWindowVisible;
        ConfigEntry<Vector4> mainWindowPosition;
        ConfigEntry<bool> infoWindowVisible;
        ConfigEntry<Vector4> infoWindowPosition;
        ConfigEntry<bool> autoStartServers;
        ConfigEntry<bool> autoAcceptConnections;
        ConfigEntry<bool> confirmRemoveClient;
        ConfigEntry<bool> pauseServerWithGame;
        ConfigEntry<string> logLevel;
        ConfigEntry<bool> verboseErrors;
        ConfigEntry<bool> oneRPCPerUpdate;
        ConfigEntry<uint> maxTimePerUpdate;
        ConfigEntry<bool> adaptiveRateControl;
        ConfigEntry<bool> blockingRecv;
        ConfigEntry<uint> recvTimeout;

        public ConfigurationFile(Configuration configuration)
        {
            Configuration = configuration;

            // FIXME: add a default server and settings for now, as there is no server UI
            configuration.Servers.Add(new Configuration.Server());
            configuration.AutoStartServers = true;
            configuration.AutoAcceptConnections = true;
            configuration.VerboseErrors = true;

            var configPath = Path.Combine(Paths.ConfigPath, "krpc2.cfg");
            configFile = new ConfigFile(configPath, false);
            configFile.SaveOnConfigSet = false;

            serverIds = configFile.Bind("Servers", "ServerIds", "");
            mainWindowVisible = configFile.Bind("UI", "MainWindowVisible", configuration.MainWindowVisible);
            mainWindowPosition = configFile.Bind("UI", "MainWindowPosition", configuration.MainWindowPosition.ToVectorF());
            infoWindowVisible = configFile.Bind("UI", "InfoWindowVisible", configuration.InfoWindowVisible);
            infoWindowPosition = configFile.Bind("UI", "InfoWindowPosition", configuration.InfoWindowPosition.ToVectorF());
            autoStartServers = configFile.Bind("Servers", "AutoStartServers", configuration.AutoStartServers);
            autoAcceptConnections = configFile.Bind("UI", "AutoAcceptConnections", configuration.AutoAcceptConnections);
            confirmRemoveClient = configFile.Bind("UI", "ConfirmRemoveClient", configuration.ConfirmRemoveClient);
            pauseServerWithGame = configFile.Bind("Servers", "PauseServerWithGame", configuration.PauseServerWithGame);
            logLevel = configFile.Bind("Logging", "LogLevel", KRPC.Utils.Logger.Level.ToString());
            verboseErrors = configFile.Bind("Logging", "VerboseErrors", configuration.VerboseErrors);
            oneRPCPerUpdate = configFile.Bind("Performance", "OneRPCPerUpdate", configuration.OneRPCPerUpdate);
            maxTimePerUpdate = configFile.Bind("Performance", "MaxTimePerUpdate", configuration.MaxTimePerUpdate);
            adaptiveRateControl = configFile.Bind("Performance", "AdaptiveRateControl", configuration.AdaptiveRateControl);
            blockingRecv = configFile.Bind("Performance", "BlockingRecv", configuration.BlockingRecv);
            recvTimeout = configFile.Bind("Performance", "RecvTimeout", configuration.RecvTimeout);

            if (File.Exists(configPath))
                Load();
            else
                Save();
        }

        public void Save()
        {
            serverIds.Value = String.Join(",", Configuration.Servers.Select(x => x.Id));

            // remove all existing server config entries
            foreach (var entries in servers.Values)
                foreach (var entry in entries)
                    configFile.Remove(entry.Definition);
            servers.Clear();
            // recreate the server config entries
            foreach (var server in Configuration.Servers)
            {
                var entries = new List<ConfigEntryBase>();
                var section = "Server-" + server.Id;
                entries.Add(configFile.Bind(section, "Name", server.Name));
                entries.Add(configFile.Bind(section, "Protocol", server.Protocol.ToString()));
                var settings = new List<string>();
                foreach (var setting in server.Settings)
                    settings.Add(setting.Key + "=" + setting.Value);
                entries.Add(configFile.Bind(section, "Settings", String.Join(",", settings)));
                servers[server.Id] = entries;
            }

            mainWindowVisible.Value = Configuration.MainWindowVisible;
            mainWindowPosition.Value = Configuration.MainWindowPosition.ToVectorF();
            infoWindowVisible.Value = Configuration.InfoWindowVisible;
            infoWindowPosition.Value = Configuration.InfoWindowPosition.ToVectorF();
            autoStartServers.Value = Configuration.AutoStartServers;
            autoAcceptConnections.Value = Configuration.AutoAcceptConnections;
            confirmRemoveClient.Value = Configuration.ConfirmRemoveClient;
            pauseServerWithGame.Value = Configuration.PauseServerWithGame;
            logLevel.Value = KRPC.Utils.Logger.Level.ToString();
            verboseErrors.Value = Configuration.VerboseErrors;
            oneRPCPerUpdate.Value = Configuration.OneRPCPerUpdate;
            maxTimePerUpdate.Value = Configuration.MaxTimePerUpdate;
            adaptiveRateControl.Value = Configuration.AdaptiveRateControl;
            blockingRecv.Value = Configuration.BlockingRecv;
            recvTimeout.Value = Configuration.RecvTimeout;
            configFile.Save();
        }

        public void Load()
        {
            // reload configuration entries for servers
            servers.Clear();
            foreach (var id in serverIds.Value.Split(',').Select(x => new Guid(x)))
            {
                var entries = new List<ConfigEntryBase>();

                var section = "Server-" + id;
                var server = new Configuration.Server();
                server.Id = id;

                ConfigEntry<string> name;
                configFile.TryGetEntry<String>(new ConfigDefinition(section, "Name"), out name);
                if (name == null)
                    name = configFile.Bind<String>(section, "Name", server.Name);
                entries.Add(name);
                server.Name = name.Value;

                ConfigEntry<string> protocol;
                configFile.TryGetEntry<String>(new ConfigDefinition(section, "Protocol"), out protocol);
                if (protocol == null)
                    protocol = configFile.Bind<String>(section, "Protocol", server.Protocol.ToString());
                entries.Add(protocol);
                try
                {
                    server.Protocol = (Protocol)Enum.Parse(typeof(Protocol), protocol.Value);
                }
                catch (ArgumentException)
                {
                    Console.WriteLine(
                        "[kRPC] Error parsing server protocol from configuration file. Got '" + protocol.Value + "'. " +
                        "Defaulting to " + Protocol.ProtocolBuffersOverTCP);
                    server.Protocol = Protocol.ProtocolBuffersOverTCP;
                }

                ConfigEntry<string> settings;
                configFile.TryGetEntry<String>(new ConfigDefinition(section, "Settings"), out settings);
                if (settings == null)
                    settings = configFile.Bind<String>(section, "Settings", "");
                entries.Add(settings);
                var settingsDictionary = new Dictionary<string, string>();
                foreach (var setting in settings.Value.Split(','))
                {
                    var parts = setting.Split('=');
                    settingsDictionary[parts[0]] = parts[1];
                }
                server.Settings = settingsDictionary;

                servers[id] = entries;
            }

            Configuration.MainWindowVisible = mainWindowVisible.Value;
            Configuration.MainWindowPosition = mainWindowPosition.Value.ToTupleF();
            Configuration.InfoWindowVisible = infoWindowVisible.Value;
            Configuration.InfoWindowPosition = infoWindowPosition.Value.ToTupleF();
            Configuration.AutoStartServers = autoStartServers.Value;
            Configuration.AutoAcceptConnections = autoAcceptConnections.Value;
            Configuration.ConfirmRemoveClient = confirmRemoveClient.Value;
            Configuration.PauseServerWithGame = pauseServerWithGame.Value;
            try
            {
                KRPC.Utils.Logger.Level = (KRPC.Utils.Logger.Severity)Enum.Parse(typeof(KRPC.Utils.Logger.Severity), logLevel.Value);
            }
            catch (ArgumentException)
            {
                Console.WriteLine(
                    "[kRPC] Error parsing log level from configuration file. Got '" + logLevel + "'. " +
                    "Defaulting to " + KRPC.Utils.Logger.Severity.Info);
                KRPC.Utils.Logger.Level = KRPC.Utils.Logger.Severity.Info;
            }
            Configuration.VerboseErrors = verboseErrors.Value;
            Configuration.OneRPCPerUpdate = oneRPCPerUpdate.Value;
            Configuration.MaxTimePerUpdate = maxTimePerUpdate.Value;
            Configuration.AdaptiveRateControl = adaptiveRateControl.Value;
            Configuration.BlockingRecv = blockingRecv.Value;
            Configuration.RecvTimeout = recvTimeout.Value;
        }
    }
}
