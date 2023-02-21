using System;

public class EventsManager
{
    public static  event Action OneWinMenuBtnClicked;
    public static  event Action OneMenuBtnClicked;
    public static  event Action OnePauseBtnClicked;
        
    public  static void SendWinMenuBtnClicked()
    {
        OneWinMenuBtnClicked?.Invoke();
    }
    
        
    public  static void SendMenuBtnClicked()
    {
        OneMenuBtnClicked?.Invoke();
    }

    public static void SendPauseBtnClicked()
    {
        OnePauseBtnClicked?.Invoke();
    }
}
