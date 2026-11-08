using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace SREXDuplicateReferenceFix;

public class Settings
{
    [SettingName("Transplant filter plugins")]
    [Tooltip("Only treat a reference as transplanted when one of these plugins appears anywhere " +
             "in the transplanted reference's override history (the winning override does not have " +
             "to be theirs). Default matches SR Exterior Cities. " +
             "Clear the list to accept transplants from any plugin.")]
    public List<string> TransplantFilterPlugins = new() { "SR Exterior Cities.esp" };

    [SettingName("Read Base Object Swapper inis")]
    [Tooltip("Scan *_SWAP.ini files in the Data folder. If the transplanted side of a pair is " +
             "disabled at runtime by a BOS transform (Initially Disabled flag or a move to " +
             "Z <= -29000), the pair is skipped so the surviving twin is left alone.")]
    public bool ReadBosSwapInis = true;

    [SettingName("Skip FormIDs")]
    [Tooltip("FormKeys that must never be disabled, in the format 09A500:Skyrim.esm. " +
             "Use this as a manual safety net on top of the automatic checks.")]
    public List<string> SkipFormIDs = new();

    [SettingName("Verbose logging")]
    [Tooltip("Log every candidate group that was inspected, including the ones that were skipped.")]
    public bool VerboseLogging = false;
}
