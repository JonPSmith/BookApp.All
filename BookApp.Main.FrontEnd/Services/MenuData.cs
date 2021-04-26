// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace BookApp.Main.FrontEnd.Services
{
    public class MenuData
    {
        public MenuData(string controllerName, string menuString)
        {
            ControllerName = controllerName;
            MenuString = menuString;
        }

        public string ControllerName { get; private set; }
        public string MenuString { get; private set; }
    }
}