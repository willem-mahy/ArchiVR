using System.Collections.Generic;
using Assets.Scripts.WM.UI;

namespace Assets.Scripts.WM.UI
{
    public class ButtonTimeOfDay : ToggleButton
    {
        // Use this for initialization
        void Start()
        {
            List<string> optionSpritePaths = new List<string>();
            optionSpritePaths.Add("Menu/LightMode/Environmental/Sundawn");
            optionSpritePaths.Add("Menu/LightMode/Environmental/Noon");
            optionSpritePaths.Add("Menu/LightMode/Environmental/Sunset");
            optionSpritePaths.Add("Menu/LightMode/Environmental/Midnight");
            LoadOptions(optionSpritePaths);
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}
