using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisskeyLiveCommentViewer
{
    internal class Osudata
    {
        OsuMemoryDataProvider.OsuMemoryReader OsuMemoryReader = new OsuMemoryDataProvider.OsuMemoryReader();
        public Osudata()
        {

        }
        public string GetOsuSongString()
        {
            return OsuMemoryReader.GetSongString();
        }
        public int GetMapSetId()
        {
            return OsuMemoryReader.GetMapSetId();
        }
        public int GetMapId()
        {
            return OsuMemoryReader.GetMapId();
        }
        public int GetRetrys()
        {
            return OsuMemoryReader.GetRetrys();
        }
        public int GetPlayingMods()
        {
            return OsuMemoryReader.GetPlayingMods();
        }
        public int GetMods()
        {
            return OsuMemoryReader.GetMods();
        }
        public OsuMemoryDataProvider.TourneyIpcState GetStatus(out int num)
        {
            return OsuMemoryReader.GetTourneyIpcState(out num);
        }
        public bool IsPlaying()
        {
            if (IsRunning()) return OsuMemoryReader.GetTourneyIpcState(out _) == OsuMemoryDataProvider.TourneyIpcState.Playing;
            return false;
        }
        public bool IsRunning()
        {
            return OsuMemoryReader.GetCurrentStatus(out _) != OsuMemoryDataProvider.OsuMemoryStatus.NotRunning;
        }
        public string GetMapURL()
        {
            return "https://osu.ppy.sh/beatmaps/" + GetMapId();
        }
        public string GetMapSetURL()
        {
            return "https://osu.ppy.sh/beatmapsets/" + GetMapSetId();
        }
        public int GetGameMode()
        {
            return OsuMemoryReader.ReadPlayedGameMode();
        }
        public int GetReatrys()
        {
            return OsuMemoryReader.GetRetrys();
        }
        public float GetHP()
        {
            return OsuMemoryReader.GetMapHp();
        }
        public float GetCS()
        {
            return OsuMemoryReader.GetMapCs();
        }
        public float GetOD()
        {
            return OsuMemoryReader.GetMapOd();
        }
        public float GetAR()
        {
            return OsuMemoryReader.GetMapAr();
        }
        public string ModString()
        {
            int num = GetMods();
            List<string> mots = new List<string>();
            for (int n = 0; n < AllMods.Length; n++)
            {
                if (IsBitSet((byte)num, n))
                {
                    mots.Add(AllMods[n].ToString());
                }
            }
            if (mots.Count > 0)
            {
                return String.Join(",", mots.ToArray());
            }
            return "none";
        }
        public string GetStars()
        {
            var tourneyLeftStars = OsuMemoryReader.ReadTourneyLeftStars();
            var tourneyRightStars = OsuMemoryReader.ReadTourneyRightStars();
            return $"{tourneyLeftStars.ToString()}.{tourneyRightStars.ToString()}";

        }
        private bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
        public Mods[] AllMods => (Mods[])System.Enum.GetValues(typeof(Mods));
    }
    public enum Mods
    {
        NF=1,
        EZ=2,
        NV=4,
        HD=8,
        HR=16,
        SD=32,
        DT=64,
        RX=128,
        HT=256,
        NC=512,
        FL=1024,
        AT=2048,
        SO=4096,
        AP=8192,
        PF=16384,
        [Description("4K")]
        fourK = 32768,
        [Description("5K")]
        fivek=65536,
        [Description("6K")]
        sixk=131072,
        [Description("7K")]
        sevennk = 262144,
        [Description("8K")]
        eightk = 524288,
        FI =1048576,
        RD=2097152,
        last_mod=4194304,
        [Description("9K")]
        ninek = 16777216,
        [Description("10K")]
        tenk =33554432,
        [Description("1K")]
        onek =67108864,
        [Description("3K")]
        threek = 134217728,
        [Description("2K")]
        twok =268435456,
    }
}
