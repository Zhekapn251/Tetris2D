using System;

public class EventsManager
{
    public static  event Action OneWinMenuBtnClicked;
        
    public  static void SendWinMenuBtnClicked()
    {
        OneWinMenuBtnClicked?.Invoke();
    }

}
