using System;


namespace MeatCountefeitDetector.Utils.Dialog.Abstract;

public interface IInteractionAware
{
    Action FinishInteraction { get; set; }
}

