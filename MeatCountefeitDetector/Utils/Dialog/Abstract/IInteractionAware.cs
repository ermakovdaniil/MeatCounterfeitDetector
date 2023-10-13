using System;


namespace MeatCounterfeitDetector.Utils.Dialog.Abstract;

public interface IInteractionAware
{
    Action FinishInteraction { get; set; }
}

