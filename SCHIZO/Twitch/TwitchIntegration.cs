﻿using System;
using System.Collections.Concurrent;
using System.IO;
using JetBrains.Annotations;
using SCHIZO.Attributes;
using SCHIZO.Helpers;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using UnityEngine;

namespace SCHIZO.Twitch;

[LoadComponent]
public sealed class TwitchIntegration : MonoBehaviour
{
    private const string OWNER_USERNAME = "alexejherodev";
    private const string TARGET_CHANNEL = "vedal987";
    private const string COMMAND_SENDER = "alexejherodev";

    private readonly TwitchClient _client;
    private readonly ConcurrentQueue<string> _msgQueue = new();

    public TwitchIntegration()
    {
        DevConsole.RegisterConsoleCommand(this, "say", true, true);

        ClientOptions clientOptions = new()
        {
            MessagesAllowedInPeriod = 750,
            ThrottlingPeriod = TimeSpan.FromSeconds(30)
        };
        WebSocketClient customClient = new(clientOptions);
        _client = new TwitchClient(customClient);

        _client.OnError += (_, evt) => LOGGER.LogError(evt.Exception);
        _client.OnIncorrectLogin += (_, evt) => LOGGER.LogError($"Could not connect: {evt.Exception.Message}");
        _client.OnConnected += (_, _) => LOGGER.LogInfo("Connected");
        _client.OnConnectionError += (_, evt) => LOGGER.LogError($"Could not connect: {evt.Error.Message}");
        _client.OnJoinedChannel += (_, _) => LOGGER.LogInfo("Joined");
        _client.OnFailureToReceiveJoinConfirmation += (_, evt) => LOGGER.LogError($"Could not join: {evt.Exception.Details}");
        _client.OnMessageReceived += Client_OnMessageReceived;

        if (!File.Exists(Path.Combine(AssetLoader.AssetsFolder, "..", "cache.json")))
        {
            LOGGER.LogWarning("Could not find cache.json for Twitch integration, it will be disabled.\n"
                + "Make a text file next to the mod .dll and put the token on the SECOND line.");
            return;
        }
        ConnectionCredentials credentials = new(OWNER_USERNAME, File.ReadAllLines(Path.Combine(AssetLoader.AssetsFolder, "..", "cache.json"))[1]);

        _client.Initialize(credentials, TARGET_CHANNEL);

        _client.Connect();
    }

    private void Client_OnMessageReceived(object _, OnMessageReceivedArgs evt)
    {
        const string PREFIX = "pls ";

        ChatMessage message = evt.ChatMessage;

        if (message.Username.ToLower() != COMMAND_SENDER) return; // ensure I don't get isekaid
        if (!message.Message.StartsWith(PREFIX)) return;

        // OnMessageReceived runs in a worker thread, where we can't use Unity APIs
        _msgQueue.Enqueue(message.Message[PREFIX.Length..]);
    }

    private void FixedUpdate()
    {
        if (_msgQueue.Count > 0 && _msgQueue.TryDequeue(out string message)) HandleMessage(message);
    }

    private void HandleMessage(string message)
    {
        MessageHelpers.SuppressOutput = true;
        DevConsole.SendConsoleCommand(message);
        MessageHelpers.SuppressOutput = false;
    }

    [UsedImplicitly]
    private void OnConsoleCommand_say(NotificationCenter.Notification n)
    {
        if (n.data.Count == 0) return;
        string message = (string)n.data[0];
        ErrorMessage.AddMessage(message);
    }
}
