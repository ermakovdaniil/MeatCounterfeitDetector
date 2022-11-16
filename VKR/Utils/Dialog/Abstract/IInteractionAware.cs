using System;


namespace VKR.Utils.Dialog.Abstract;

public interface IInteractionAware
{
    Action FinishInteraction { get; set; }
}
