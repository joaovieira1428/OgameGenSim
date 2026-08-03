namespace OgameGenSim.Classes.Views;

public class LoadingQuotes
{
    private static readonly Random _random = new();

    public static readonly string[] Quotes =
    {
        "Negotiating with pirates...",
        "Mining asteroids for spare bytes...",
        "Refueling the recyclers...",
        "Convincing the moon to stay put...",
        "Teaching probes to probe...",
        "Calculating improbable trajectories...",
        "Scanning for life... still none.",
        "Dodging space paperwork...",
        "Counting debris... again.",
        "Training the fleet not to miss.",
        "Asking the commander for permission...",
        "Looking for free parking in orbit.",
        "Training stormtroopers...",
        "Rolling suspiciously fair dice...",
        "Teaching lasers to pew...",
        "Buffing shields with elbow grease...",
        "Explaining physics to missiles...",
        "Repairing emotional damage...",
        "Hiring replacement pilots...",
        "Repainting the explosions...",
        "Loading dramatic music...",
        "Making the battle look cooler...",
        "Simulating heroic last stands...",
        "Calculating \"trust me bro\" odds...",
        "Summoning rubber ducks...",
        "Asking the void for directions...",
        "Rotating the Earth slightly...",
        "Reversing the polarity of the waffles...",
        "Untangling spaghetti code...",
        "Appeasing the ancient compiler gods...",
        "Teaching cats to fetch stack traces...",
        "Looking for the Any key...",
        "Chasing escaped semicolons...",
        "Bribing the random number generator...",
        "Measuring the immeasurable...",
        "Watering the silicon...",
        "Charging the wireless cables...",
        "Folding space... carefully.",
        "Pretending to work really hard...",
        "Almost there™",
        "This message intentionally left loading.",
        "You look nice today.",
        "If this takes too long, blame the intern.",
        "Have you tried turning the galaxy off and on again?",
        "Definitely not generating these messages on the fly.",
        "This spinner is spinning at maximum efficiency.",
        "The progress bar is emotionally unavailable.",
        "Fun fact: Loading screens increase patience by 0%.",
        "Please act surprised when this finishes.",
        "Tip: Staring at the spinner makes it slower."
    };

    public static string GetRandomQuote()
    {
        return Quotes[_random.Next(Quotes.Length)];
    }
}
