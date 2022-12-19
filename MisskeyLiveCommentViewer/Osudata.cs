using System;
using System.Collections.Generic;
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
            for(int n=0;n < AllMods.Length; n++) 
            { 
                if (IsBitSet((byte)num,n))
                {
                    mots.Add(AllMods[n].ToString());
                }
            }
            if(mots.Count > 0)
            {
                return String.Join(",",mots.ToArray());
            }
            return "none";
        }
        private bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
        public Mods[] AllMods => (Mods[])System.Enum.GetValues(typeof(Mods));
    }
    public enum Mods
    {
        no_fail=1,
        easy=2,
        no_video=4,
        hidden=8,
        hard_rock=16,
        sudden_death=32,
        double_time=64,
        relax=128,
        half_time=256,
        nightcore=512,
        flashlight=1024,
        autoplay=2048,
        spin_out=4096,
        relax2=8192,
        perfect=16384,
        key4=32768,
        key5=65536,
        key6=131072,
        key7=262144,
        key8=524288,
        fade_in=1048576,
        random=2097152,
        last_mod=4194304,
        key9=16777216,
        key10=33554432,
        key1=67108864,
        key3=134217728,
        key2=268435456,
    }
}
