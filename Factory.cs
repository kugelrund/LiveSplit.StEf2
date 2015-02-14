using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;

namespace LiveSplit.StEf2
{
    public class Factory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "Star Trek: Elite Force 2 Auto Splitter"; }
        }
        public ComponentCategory Category
        {
            get { return ComponentCategory.Control; }
        }
        public string Description
        {
            get { return "Automates splitting for Star Trek: Elite Force 2 and allows to remove loadtimes."; }
        }
        public IComponent Create(LiveSplitState state)
        {
            return new Component(state);
        }
        public string UpdateName
        {
            get { return ComponentName; }
        }
        public string UpdateURL
        {
            get { return "https://raw.githubusercontent.com/kugelrund/LiveSplit.StEf2/master/"; }
        }
        public Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }
        public string XMLURL
        {
            get { return UpdateURL + "Components/update.LiveSplit.StEf2.xml"; }
        }
    }
}