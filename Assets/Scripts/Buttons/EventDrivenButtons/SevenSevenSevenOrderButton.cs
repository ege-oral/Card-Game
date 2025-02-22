using Buttons.EventDrivenButtons.Base;
using Buttons.Signals;

namespace Buttons.EventDrivenButtons
{
    public class SevenSevenSevenOrderButton : EventDrivenButton
    {
        protected override void OnButtonClicked()
        {
            SignalBus.Fire<SevenSevenSevenOrderSignal>();
        }
    }
}