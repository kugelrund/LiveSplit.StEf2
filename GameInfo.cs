using System;
using System.Diagnostics;
using LiveSplit.ComponentUtil;

namespace LiveSplit.StEf2
{
    class GameInfo
    {
        // 1 - main menu, titlescreen and so on
        // 4 - load state (only when loading first level)
        // 5 - load state (at the beginning of each loading phase)
        // 6 - main loading state
        // 7 - end loading state
        // 8 - in game
        private static readonly DeepPointer gameStateAddress = new DeepPointer(0x2A6D4, 0);

        private static readonly DeepPointer mapAddress = new DeepPointer(0xF758C, 0);


        // longest map name is forgeboss
        private const int MAX_MAP_LENGTH = 32;

        private Process gameProcess;

        public int PrevGameState { get; private set; }
        public int CurrGameState { get; private set; }
        public string PrevMap { get; private set; }
        public string CurrMap { get; private set; }
        public bool MapChanged { get; private set; }
        public bool InGame { get; private set; }


        public GameInfo(Process gameProcess)
        {
            this.gameProcess = gameProcess;
        }

        private void UpdateMap()
        {
            string map = mapAddress.DerefString(gameProcess, MAX_MAP_LENGTH);
            if (map != null && map != CurrMap)
            {
                PrevMap = CurrMap;
                CurrMap = map;
                MapChanged = true;
            }
        }

        public void Update()
        {
            PrevGameState = CurrGameState;
            int currGameState;
            if (gameStateAddress.Deref<int>(gameProcess, out currGameState))
            {
                CurrGameState = currGameState;
            }

            if (PrevGameState != CurrGameState)
            {
                UpdateMap();
                InGame = (CurrGameState == 8);
            }
            else
            {
                MapChanged = false;
            }
        }
    }

    abstract class GameEvent : IComparable<GameEvent>
    {
        private static GameEvent[] eventList = null;

        private int order = -1;
        public int Order { get { return order; } }
        public abstract string Id { get; }

        public static GameEvent[] GetEventList()
        {
            if (eventList == null)
            {
                eventList = new GameEvent[] { 
                    #region events
                    new StartedGameEvent(),
                    new LoadedMapEvent("m0-intro"),
                    new FinishedMapEvent("m0-intro"),
                    new LoadedMapEvent("m1l1a-borg_sphere"),
                    new FinishedMapEvent("m1l1a-borg_sphere"),
                    new LoadedMapEvent("m1l1b-borg_sphere"),
                    new FinishedMapEvent("m1l1b-borg_sphere"),
                    new LoadedMapEvent("m1l1c-borg_sphere"),
                    new FinishedMapEvent("m1l1c-borg_sphere"),
                    new LoadedMapEvent("m1l2a-borg_sphere"),
                    new FinishedMapEvent("m1l2a-borg_sphere"),
                    new LoadedMapEvent("m1l2c-borg_sphere"),
                    new FinishedMapEvent("m1l2c-borg_sphere"),
                    new LoadedMapEvent("m1l2b-borg_sphere"),
                    new FinishedMapEvent("m1l2b-borg_sphere"),
                    new LoadedMapEvent("m1l3-borg_boss"),
                    new FinishedMapEvent("m1l3-borg_boss"),
                    new LoadedMapEvent("m2l0-sfa"),
                    new FinishedMapEvent("m2l0-sfa"),
                    new LoadedMapEvent("m2l1-sfa"),
                    new FinishedMapEvent("m2l1-sfa"),
                    new LoadedMapEvent("m2l2-sfa"),
                    new FinishedMapEvent("m2l2-sfa"),
                    new LoadedMapEvent("ent-training_weap1"),
                    new FinishedMapEvent("ent-training_weap1"),
                    new LoadedMapEvent("m3l1a-forever"),
                    new FinishedMapEvent("m3l1a-forever"),
                    new LoadedMapEvent("m3l1b-forever"),
                    new FinishedMapEvent("m3l1b-forever"),
                    new LoadedMapEvent("m3l2-forever"),
                    new FinishedMapEvent("m3l2-forever"),
                    new LoadedMapEvent("m4l1a-attrexian_station"),
                    new FinishedMapEvent("m4l1a-attrexian_station"),
                    new LoadedMapEvent("m4l1b-attrexian_station"),
                    new FinishedMapEvent("m4l1b-attrexian_station"),
                    new LoadedMapEvent("m4l2b-attrexian_station"),
                    new FinishedMapEvent("m4l2b-attrexian_station"),
                    new LoadedMapEvent("m5l1a-drull_ruins1"),
                    new FinishedMapEvent("m5l1a-drull_ruins1"),
                    new LoadedMapEvent("m5l1b-drull_ruins1"),
                    new FinishedMapEvent("m5l1b-drull_ruins1"),
                    new LoadedMapEvent("m5l2a-drull_ruins1"),
                    new FinishedMapEvent("m5l2a-drull_ruins1"),
                    new LoadedMapEvent("m5l2b-drull_ruins1"),
                    new FinishedMapEvent("m5l2b-drull_ruins1"),
                    new LoadedMapEvent("m5l2c-drull_ruins1"),
                    new FinishedMapEvent("m5l2c-drull_ruins1"),
                    new LoadedMapEvent("ent-training_weap2"),
                    new FinishedMapEvent("ent-training_weap2"),
                    new LoadedMapEvent("m6l0-attack"),
                    new FinishedMapEvent("m6l0-attack"),
                    new LoadedMapEvent("m6-deck8_redalert"),
                    new FinishedMapEvent("m6-deck8_redalert"),
                    new LoadedMapEvent("m6-deck16_engineering"),
                    new FinishedMapEvent("m6-deck16_engineering"),
                    new LoadedMapEvent("m6-deck1_bridge_redalert"),
                    new FinishedMapEvent("m6-deck1_bridge_redalert"),
                    new LoadedMapEvent("m6-exterior"),
                    new FinishedMapEvent("m6-exterior"),
                    new LoadedMapEvent("m7l1a-attrexian_colony"),
                    new FinishedMapEvent("m7l1a-attrexian_colony"),
                    new LoadedMapEvent("m7l1b-attrexian_colony"),
                    new FinishedMapEvent("m7l1b-attrexian_colony"),
                    new LoadedMapEvent("m7l2a-attrexian_colony"),
                    new FinishedMapEvent("m7l2a-attrexian_colony"),
                    new LoadedMapEvent("m7l2b-attrexian_colony"),
                    new FinishedMapEvent("m7l2b-attrexian_colony"),
                    new LoadedMapEvent("m7l2c-attrexian_colony"),
                    new FinishedMapEvent("m7l2c-attrexian_colony"),
                    new LoadedMapEvent("m8l1a-drull_ruins2"),
                    new FinishedMapEvent("m8l1a-drull_ruins2"),
                    new LoadedMapEvent("m8l1b-drull_ruins2"),
                    new FinishedMapEvent("m8l1b-drull_ruins2"),
                    new LoadedMapEvent("m8l2a-drull_ruins2"),
                    new FinishedMapEvent("m8l2a-drull_ruins2"),
                    new LoadedMapEvent("m8l2b-drull_ruins2"),
                    new FinishedMapEvent("m8l2b-drull_ruins2"),
                    new LoadedMapEvent("ent-training_weap3"),
                    new FinishedMapEvent("ent-training_weap3"),
                    new LoadedMapEvent("m9l1a-klingon_base"),
                    new FinishedMapEvent("m9l1a-klingon_base"),
                    new LoadedMapEvent("m9l1b-klingon_base"),
                    new FinishedMapEvent("m9l1b-klingon_base"),
                    new LoadedMapEvent("m9l2-klingon_base"),
                    new FinishedMapEvent("m9l2-klingon_base"),
                    new LoadedMapEvent("ent-training_weap4"),
                    new FinishedMapEvent("ent-training_weap4"),
                    new LoadedMapEvent("m10l1-romulan_installation"),
                    new FinishedMapEvent("m10l1-romulan_installation"),
                    new LoadedMapEvent("m10l2a-romulan_installation"),
                    new FinishedMapEvent("m10l2a-romulan_installation"),
                    new LoadedMapEvent("m10l2b-romulan_installation"),
                    new FinishedMapEvent("m10l2b-romulan_installation"),
                    new LoadedMapEvent("m11l1a-drull_ruins3"),
                    new FinishedMapEvent("m11l1a-drull_ruins3"),
                    new LoadedMapEvent("m11l2a-drull_ruins3"),
                    new FinishedMapEvent("m11l2a-drull_ruins3"),
                    new LoadedMapEvent("m11l3a-drull_ruins3_boss"),
                    new FinishedMapEvent("m11l3a-drull_ruins3_boss"),
                    new LoadedMapEvent("m11l3b-drull_ruins3_boss"),
                    new FinishedMapEvent("m11l3b-drull_ruins3_boss"),
                    new LoadedMapEvent("m12-end"),
                    new FinishedMapEvent("m12-end"),
                    new LoadedMapEvent("credits"),
                    new FinishedMapEvent("credits")
                    #endregion
                };
                for (int i = 0; i < eventList.Length; ++i)
                {
                    eventList[i].order = i;
                }
            }

            return eventList;
        }

        public int CompareTo(GameEvent other)
        {
            if (order == -1 || other.order == -1)
            {
                throw new ArgumentException();
            }
            else
            {
                return order - other.order;
            }
        }

        public abstract bool HasOccured(GameInfo info);
    }

    class StartedGameEvent : GameEvent
    {
        public override string Id { get { return "new_game_started"; } }

        public override bool HasOccured(GameInfo info)
        {
            return info.CurrGameState == 4;
        }

        public override string ToString()
        {
            return "Started a new game";
        }
    }

    abstract class MapEvent : GameEvent
    {
        protected readonly string map;

        public MapEvent(string map)
        {
            this.map = map;
        }
    }

    class LoadedMapEvent : MapEvent
    {
        public override string Id { get { return "loaded_map_" + map; } }

        public LoadedMapEvent(string map) : base(map) { }

        public override bool HasOccured(GameInfo info)
        {
            return (info.PrevGameState != 8) && info.InGame && (info.CurrMap == map);
        }
                
        public override string ToString()
        {
            return "Loaded '" + map + "'";
        }
    }

    class FinishedMapEvent : MapEvent
    {
        public override string Id { get { return "finished_map_" + map; } }

        public FinishedMapEvent(string map) : base(map) { }

        public override bool HasOccured(GameInfo info)
        {
            return info.MapChanged && (info.CurrMap != map) && (info.PrevMap == map);
        }

        public override string ToString()
        {
            return "Finished '" + map + "'";
        }
    }

    class EmptyEvent : GameEvent
    {
        public override string Id { get { return "empty"; } }

        public override bool HasOccured(GameInfo info)
        {
            return false;
        }
    }
}
