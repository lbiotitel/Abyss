using UnityEngine;

public enum EVENT_TYPE
{ 
    GAME_INIT, 
    GAME_END, 
    ITEM_EQUIP,
    HEALTH_CHANGE, 
    DEAD 
};

public interface IListener
{
    void OnEvent(EVENT_TYPE Event_Type, Component Sender, object Param = null);
}
