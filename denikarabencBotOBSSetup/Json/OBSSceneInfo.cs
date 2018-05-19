// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using denikarabencBotOBSSetup.Json;
//
//    var obsSceneInfo = ObsSceneInfo.FromJson(jsonString);

namespace denikarabencBotOBSSetup.Json
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class OBSSceneInfo
    {
        [JsonProperty("AuxAudioDevice1")]
        public AudioDevice1 AuxAudioDevice1 { get; set; }

        [JsonProperty("DesktopAudioDevice1")]
        public AudioDevice1 DesktopAudioDevice1 { get; set; }

        [JsonProperty("current_program_scene")]
        public string CurrentProgramScene { get; set; }

        [JsonProperty("current_scene")]
        public string CurrentScene { get; set; }

        [JsonProperty("current_transition")]
        public string CurrentTransition { get; set; }

        [JsonProperty("modules")]
        public Modules Modules { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("preview_locked")]
        public bool PreviewLocked { get; set; }

        [JsonProperty("quick_transitions")]
        public List<QuickTransition> QuickTransitions { get; set; }

        [JsonProperty("saved_multiview_projectors")]
        public List<SavedMultiviewProjector> SavedMultiviewProjectors { get; set; }

        [JsonProperty("saved_preview_projectors")]
        public List<SavedPreviewProjector> SavedPreviewProjectors { get; set; }

        [JsonProperty("saved_projectors")]
        public List<SavedProjector> SavedProjectors { get; set; }

        [JsonProperty("saved_studio_preview_projectors")]
        public List<SavedStudioPreviewProjector> SavedStudioPreviewProjectors { get; set; }

        [JsonProperty("scaling_enabled")]
        public bool ScalingEnabled { get; set; }

        [JsonProperty("scaling_level")]
        public long ScalingLevel { get; set; }

        [JsonProperty("scaling_off_x")]
        public long ScalingOffX { get; set; }

        [JsonProperty("scaling_off_y")]
        public long ScalingOffY { get; set; }

        [JsonProperty("scene_order")]
        public List<SceneOrder> SceneOrder { get; set; }

        [JsonProperty("sources")]
        public List<Source> Sources { get; set; }

        [JsonProperty("transition_duration")]
        public long TransitionDuration { get; set; }

        [JsonProperty("transitions")]
        public List<object> Transitions { get; set; }
    }

    public partial class AudioDevice1
    {
        [JsonProperty("deinterlace_field_order")]
        public long DeinterlaceFieldOrder { get; set; }

        [JsonProperty("deinterlace_mode")]
        public long DeinterlaceMode { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("filters", NullValueHandling = NullValueHandling.Ignore)]
        public List<Filter> Filters { get; set; }

        [JsonProperty("flags")]
        public long Flags { get; set; }

        [JsonProperty("hotkeys")]
        public AuxAudioDevice1Hotkeys Hotkeys { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mixers")]
        public long Mixers { get; set; }

        [JsonProperty("monitoring_type")]
        public long MonitoringType { get; set; }

        [JsonProperty("muted")]
        public bool Muted { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("private_settings")]
        public PrivateSettings PrivateSettings { get; set; }

        [JsonProperty("push-to-mute")]
        public bool PushToMute { get; set; }

        [JsonProperty("push-to-mute-delay")]
        public long PushToMuteDelay { get; set; }

        [JsonProperty("push-to-talk")]
        public bool PushToTalk { get; set; }

        [JsonProperty("push-to-talk-delay")]
        public long PushToTalkDelay { get; set; }

        [JsonProperty("settings")]
        public AuxAudioDevice1Settings Settings { get; set; }

        [JsonProperty("sync")]
        public long Sync { get; set; }

        [JsonProperty("volume")]
        public double Volume { get; set; }
    }

    public partial class Filter
    {
        [JsonProperty("deinterlace_field_order")]
        public long DeinterlaceFieldOrder { get; set; }

        [JsonProperty("deinterlace_mode")]
        public long DeinterlaceMode { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("flags")]
        public long Flags { get; set; }

        [JsonProperty("hotkeys")]
        public PrivateSettings Hotkeys { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mixers")]
        public long Mixers { get; set; }

        [JsonProperty("monitoring_type")]
        public long MonitoringType { get; set; }

        [JsonProperty("muted")]
        public bool Muted { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("private_settings")]
        public PrivateSettings PrivateSettings { get; set; }

        [JsonProperty("push-to-mute")]
        public bool PushToMute { get; set; }

        [JsonProperty("push-to-mute-delay")]
        public long PushToMuteDelay { get; set; }

        [JsonProperty("push-to-talk")]
        public bool PushToTalk { get; set; }

        [JsonProperty("push-to-talk-delay")]
        public long PushToTalkDelay { get; set; }

        [JsonProperty("settings")]
        public FilterSettings Settings { get; set; }

        [JsonProperty("sync")]
        public long Sync { get; set; }

        [JsonProperty("volume")]
        public long Volume { get; set; }
    }

    public partial class PrivateSettings
    {
    }

    public partial class FilterSettings
    {
        [JsonProperty("attack_time", NullValueHandling = NullValueHandling.Ignore)]
        public long? AttackTime { get; set; }

        [JsonProperty("output_gain", NullValueHandling = NullValueHandling.Ignore)]
        public double? OutputGain { get; set; }

        [JsonProperty("threshold", NullValueHandling = NullValueHandling.Ignore)]
        public double? Threshold { get; set; }

        [JsonProperty("suppress_level", NullValueHandling = NullValueHandling.Ignore)]
        public long? SuppressLevel { get; set; }

        [JsonProperty("close_threshold", NullValueHandling = NullValueHandling.Ignore)]
        public long? CloseThreshold { get; set; }

        [JsonProperty("open_threshold", NullValueHandling = NullValueHandling.Ignore)]
        public long? OpenThreshold { get; set; }

        [JsonProperty("db", NullValueHandling = NullValueHandling.Ignore)]
        public double? Db { get; set; }

        [JsonProperty("chunk_data", NullValueHandling = NullValueHandling.Ignore)]
        public string ChunkData { get; set; }

        [JsonProperty("open_when_active_vst_settings", NullValueHandling = NullValueHandling.Ignore)]
        public bool? OpenWhenActiveVstSettings { get; set; }

        [JsonProperty("plugin_path", NullValueHandling = NullValueHandling.Ignore)]
        public string PluginPath { get; set; }
    }

    public partial class AuxAudioDevice1Hotkeys
    {
        [JsonProperty("libobs.mute")]
        public List<LibobsMute> LibobsMute { get; set; }

        [JsonProperty("libobs.push-to-mute")]
        public List<object> LibobsPushToMute { get; set; }

        [JsonProperty("libobs.push-to-talk")]
        public List<object> LibobsPushToTalk { get; set; }

        [JsonProperty("libobs.unmute")]
        public List<LibobsMute> LibobsUnmute { get; set; }
    }

    public partial class LibobsMute
    {
        [JsonProperty("alt")]
        public bool Alt { get; set; }

        [JsonProperty("control")]
        public bool Control { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("shift")]
        public bool Shift { get; set; }
    }

    public partial class AuxAudioDevice1Settings
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }
    }

    public partial class Modules
    {
        [JsonProperty("auto-scene-switcher")]
        public AutoSceneSwitcher AutoSceneSwitcher { get; set; }

        [JsonProperty("captions")]
        public Captions Captions { get; set; }

        [JsonProperty("output-timer")]
        public OutputTimer OutputTimer { get; set; }

        [JsonProperty("scripts-tool")]
        public List<object> ScriptsTool { get; set; }
    }

    public partial class AutoSceneSwitcher
    {
        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("interval")]
        public long Interval { get; set; }

        [JsonProperty("non_matching_scene")]
        public string NonMatchingScene { get; set; }

        [JsonProperty("switch_if_not_matching")]
        public bool SwitchIfNotMatching { get; set; }

        [JsonProperty("switches")]
        public List<object> Switches { get; set; }
    }

    public partial class Captions
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("lang_id")]
        public long LangId { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }
    }

    public partial class OutputTimer
    {
        [JsonProperty("autoStartRecordTimer")]
        public bool AutoStartRecordTimer { get; set; }

        [JsonProperty("autoStartStreamTimer")]
        public bool AutoStartStreamTimer { get; set; }

        [JsonProperty("recordTimerHours")]
        public long RecordTimerHours { get; set; }

        [JsonProperty("recordTimerMinutes")]
        public long RecordTimerMinutes { get; set; }

        [JsonProperty("recordTimerSeconds")]
        public long RecordTimerSeconds { get; set; }

        [JsonProperty("streamTimerHours")]
        public long StreamTimerHours { get; set; }

        [JsonProperty("streamTimerMinutes")]
        public long StreamTimerMinutes { get; set; }

        [JsonProperty("streamTimerSeconds")]
        public long StreamTimerSeconds { get; set; }
    }

    public partial class QuickTransition
    {
        [JsonProperty("duration")]
        public long Duration { get; set; }

        [JsonProperty("hotkeys")]
        public List<object> Hotkeys { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class SavedMultiviewProjector
    {
        [JsonProperty("saved_multiview_projectors")]
        public long SavedMultiviewProjectors { get; set; }
    }

    public partial class SavedPreviewProjector
    {
        [JsonProperty("saved_preview_projectors")]
        public long SavedPreviewProjectors { get; set; }
    }

    public partial class SavedProjector
    {
        [JsonProperty("saved_projectors")]
        public SavedProjectors SavedProjectors { get; set; }
    }

    public partial class SavedStudioPreviewProjector
    {
        [JsonProperty("saved_studio_preview_projectors")]
        public long SavedStudioPreviewProjectors { get; set; }
    }

    public partial class SceneOrder
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Source
    {
        [JsonProperty("deinterlace_field_order")]
        public long DeinterlaceFieldOrder { get; set; }

        [JsonProperty("deinterlace_mode")]
        public long DeinterlaceMode { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("flags")]
        public long Flags { get; set; }

        [JsonProperty("hotkeys")]
        public SourceHotkeys Hotkeys { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("mixers")]
        public long Mixers { get; set; }

        [JsonProperty("monitoring_type")]
        public long MonitoringType { get; set; }

        [JsonProperty("muted")]
        public bool Muted { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("private_settings")]
        public PrivateSettings PrivateSettings { get; set; }

        [JsonProperty("push-to-mute")]
        public bool PushToMute { get; set; }

        [JsonProperty("push-to-mute-delay")]
        public long PushToMuteDelay { get; set; }

        [JsonProperty("push-to-talk")]
        public bool PushToTalk { get; set; }

        [JsonProperty("push-to-talk-delay")]
        public long PushToTalkDelay { get; set; }

        [JsonProperty("settings")]
        public SourceSettings Settings { get; set; }

        [JsonProperty("sync")]
        public long Sync { get; set; }

        [JsonProperty("volume")]
        public long Volume { get; set; }
    }

    public partial class SourceHotkeys
    {
        [JsonProperty("OBSBasic.SelectScene", NullValueHandling = NullValueHandling.Ignore)]
        public List<LibobsMute> ObsBasicSelectScene { get; set; }

        [JsonProperty("libobs.hide_scene_item.Image", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemImage { get; set; }

        [JsonProperty("libobs.hide_scene_item.Starting soon text", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemStartingSoonText { get; set; }

        [JsonProperty("libobs.hide_scene_item.Text (GDI+)", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemTextGdi { get; set; }

        [JsonProperty("libobs.hide_scene_item.twitter", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemTwitter { get; set; }

        [JsonProperty("libobs.show_scene_item.Image", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemImage { get; set; }

        [JsonProperty("libobs.show_scene_item.Starting soon text", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemStartingSoonText { get; set; }

        [JsonProperty("libobs.show_scene_item.Text (GDI+)", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemTextGdi { get; set; }

        [JsonProperty("libobs.show_scene_item.twitter", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemTwitter { get; set; }

        [JsonProperty("libobs.hide_scene_item.Chat box", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemChatBox { get; set; }

        [JsonProperty("libobs.hide_scene_item.Follow notification", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemFollowNotification { get; set; }

        [JsonProperty("libobs.hide_scene_item.Game Capture", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemGameCapture { get; set; }

        [JsonProperty("libobs.hide_scene_item.denikarabencBot Replay", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemDenikarabencBotReplay { get; set; }

        [JsonProperty("libobs.hide_scene_item.denikarabencBot Twitch Clip", NullValueHandling = NullValueHandling.Ignore)]
        public List<LibobsSceneItemDenikarabencBotTwitchClip> LibobsHideSceneItemDenikarabencBotTwitchClip { get; set; }

        [JsonProperty("libobs.show_scene_item.Chat box", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemChatBox { get; set; }

        [JsonProperty("libobs.show_scene_item.Follow notification", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemFollowNotification { get; set; }

        [JsonProperty("libobs.show_scene_item.Game Capture", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemGameCapture { get; set; }

        [JsonProperty("libobs.show_scene_item.denikarabencBot Replay", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemDenikarabencBotReplay { get; set; }

        [JsonProperty("libobs.show_scene_item.denikarabencBot Twitch Clip", NullValueHandling = NullValueHandling.Ignore)]
        public List<LibobsSceneItemDenikarabencBotTwitchClip> LibobsShowSceneItemDenikarabencBotTwitchClip { get; set; }

        [JsonProperty("libobs.hide_scene_item.OnHold", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemOnHold { get; set; }

        [JsonProperty("libobs.hide_scene_item.spongeBob", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemSpongeBob { get; set; }

        [JsonProperty("libobs.show_scene_item.OnHold", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemOnHold { get; set; }

        [JsonProperty("libobs.show_scene_item.spongeBob", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemSpongeBob { get; set; }

        [JsonProperty("libobs.hide_scene_item.Stream is ending", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemStreamIsEnding { get; set; }

        [JsonProperty("libobs.hide_scene_item.Thank you for watching!", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemThankYouForWatching { get; set; }

        [JsonProperty("libobs.show_scene_item.Stream is ending", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemStreamIsEnding { get; set; }

        [JsonProperty("libobs.show_scene_item.Thank you for watching!", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemThankYouForWatching { get; set; }

        [JsonProperty("hotkey_start", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> HotkeyStart { get; set; }

        [JsonProperty("hotkey_stop", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> HotkeyStop { get; set; }

        [JsonProperty("libobs.hide_scene_item.Display Capture", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsHideSceneItemDisplayCapture { get; set; }

        [JsonProperty("libobs.show_scene_item.Display Capture", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> LibobsShowSceneItemDisplayCapture { get; set; }
    }

    public partial class LibobsSceneItemDenikarabencBotTwitchClip
    {
        [JsonProperty("key")]
        public string Key { get; set; }
    }

    public partial class SourceSettings
    {
        [JsonProperty("fps", NullValueHandling = NullValueHandling.Ignore)]
        public long? Fps { get; set; }

        [JsonProperty("height", NullValueHandling = NullValueHandling.Ignore)]
        public long? Height { get; set; }

        [JsonProperty("is_local_file", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsLocalFile { get; set; }

        [JsonProperty("local_file", NullValueHandling = NullValueHandling.Ignore)]
        public string LocalFile { get; set; }

        [JsonProperty("restart_when_active", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RestartWhenActive { get; set; }

        [JsonProperty("shutdown", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Shutdown { get; set; }

        [JsonProperty("width", NullValueHandling = NullValueHandling.Ignore)]
        public long? Width { get; set; }

        [JsonProperty("cursor", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Cursor { get; set; }

        [JsonProperty("priority", NullValueHandling = NullValueHandling.Ignore)]
        public long? Priority { get; set; }

        [JsonProperty("window", NullValueHandling = NullValueHandling.Ignore)]
        public string Window { get; set; }

        [JsonProperty("outline", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Outline { get; set; }

        [JsonProperty("outline_color", NullValueHandling = NullValueHandling.Ignore)]
        public long? OutlineColor { get; set; }

        [JsonProperty("outline_size", NullValueHandling = NullValueHandling.Ignore)]
        public long? OutlineSize { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [JsonProperty("file", NullValueHandling = NullValueHandling.Ignore)]
        public string File { get; set; }

        [JsonProperty("align", NullValueHandling = NullValueHandling.Ignore)]
        public string Align { get; set; }

        [JsonProperty("bk_opacity", NullValueHandling = NullValueHandling.Ignore)]
        public long? BkOpacity { get; set; }

        [JsonProperty("valign", NullValueHandling = NullValueHandling.Ignore)]
        public string Valign { get; set; }

        [JsonProperty("id_counter", NullValueHandling = NullValueHandling.Ignore)]
        public long? IdCounter { get; set; }

        [JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        public List<Item> Items { get; set; }

        [JsonProperty("outline_opacity", NullValueHandling = NullValueHandling.Ignore)]
        public long? OutlineOpacity { get; set; }

        [JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("align")]
        public long Align { get; set; }

        [JsonProperty("bounds")]
        public Bounds Bounds { get; set; }

        [JsonProperty("bounds_align")]
        public long BoundsAlign { get; set; }

        [JsonProperty("bounds_type")]
        public long BoundsType { get; set; }

        [JsonProperty("crop_bottom")]
        public long CropBottom { get; set; }

        [JsonProperty("crop_left")]
        public long CropLeft { get; set; }

        [JsonProperty("crop_right")]
        public long CropRight { get; set; }

        [JsonProperty("crop_top")]
        public long CropTop { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pos")]
        public Bounds Pos { get; set; }

        [JsonProperty("private_settings")]
        public PrivateSettings PrivateSettings { get; set; }

        [JsonProperty("rot")]
        public long Rot { get; set; }

        [JsonProperty("scale")]
        public Bounds Scale { get; set; }

        [JsonProperty("scale_filter")]
        public ScaleFilter ScaleFilter { get; set; }

        [JsonProperty("visible")]
        public bool Visible { get; set; }
    }

    public partial class Bounds
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }
    }

    public enum SavedProjectors { Empty };

    public enum ScaleFilter { Disable };

    public partial class OBSSceneInfo
    {
        public static OBSSceneInfo FromJson(string json) => JsonConvert.DeserializeObject<OBSSceneInfo>(json, denikarabencBotOBSSetup.Json.Converter.Settings);
    }

    static class SavedProjectorsExtensions
    {
        public static SavedProjectors? ValueForString(string str)
        {
            switch (str)
            {
                case "": return SavedProjectors.Empty;
                default: return null;
            }
        }

        public static SavedProjectors ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this SavedProjectors value, JsonWriter writer, JsonSerializer serializer)
        {
            switch (value)
            {
                case SavedProjectors.Empty: serializer.Serialize(writer, ""); break;
            }
        }
    }

    static class ScaleFilterExtensions
    {
        public static ScaleFilter? ValueForString(string str)
        {
            switch (str)
            {
                case "disable": return ScaleFilter.Disable;
                default: return null;
            }
        }

        public static ScaleFilter ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this ScaleFilter value, JsonWriter writer, JsonSerializer serializer)
        {
            switch (value)
            {
                case ScaleFilter.Disable: serializer.Serialize(writer, "disable"); break;
            }
        }
    }

    public static class Serialize
    {
        public static string ToJson(this OBSSceneInfo self) => JsonConvert.SerializeObject(self, denikarabencBotOBSSetup.Json.Converter.Settings);
    }

    internal class Converter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(SavedProjectors) || t == typeof(ScaleFilter) || t == typeof(SavedProjectors?) || t == typeof(ScaleFilter?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (t == typeof(SavedProjectors))
                return SavedProjectorsExtensions.ReadJson(reader, serializer);
            if (t == typeof(ScaleFilter))
                return ScaleFilterExtensions.ReadJson(reader, serializer);
            if (t == typeof(SavedProjectors?))
            {
                if (reader.TokenType == JsonToken.Null) return null;
                return SavedProjectorsExtensions.ReadJson(reader, serializer);
            }
            if (t == typeof(ScaleFilter?))
            {
                if (reader.TokenType == JsonToken.Null) return null;
                return ScaleFilterExtensions.ReadJson(reader, serializer);
            }
            throw new Exception("Unknown type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = value.GetType();
            if (t == typeof(SavedProjectors))
            {
                ((SavedProjectors)value).WriteJson(writer, serializer);
                return;
            }
            if (t == typeof(ScaleFilter))
            {
                ((ScaleFilter)value).WriteJson(writer, serializer);
                return;
            }
            throw new Exception("Unknown type");
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new Converter(),
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
