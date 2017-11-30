using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchBot.Helpers
{
    public class MessageRepository
    {
        private List<string> loadingMessages;
        private List<string> errorMessages;

        public MessageRepository()
        {
            loadingMessages = new List<string>();
            PopulateLoadingMessages(loadingMessages);

            errorMessages = new List<string>();
            PopulateErrorMessages(errorMessages);
        }

        public List<string> LoadingMessages => loadingMessages;
        public List<string> ErrorMessages => errorMessages;

        private void PopulateErrorMessages(List<string> errorMessages)
        {
            errorMessages.Add("Wow, it failed miserably FeelsBadMan");
            errorMessages.Add("It crashed and burned");
            errorMessages.Add("Not gonna happen");
            errorMessages.Add("Well, this is embarrassing...");
            errorMessages.Add("PC is just not powerful enough to complete that action");
            errorMessages.Add("Need more APM for that");
            errorMessages.Add("My goose is cooked!");
            errorMessages.Add("Let's pretend that didn't happen");
            errorMessages.Add("Accidently pressed alt + F4, try again");
            errorMessages.Add("Focus on good things, not bad like this one");
            errorMessages.Add("I would like that to work as well");
            errorMessages.Add("Hope streamer haven't seen that monkaS");
            errorMessages.Add("Forgot my glasses so I cannot do that");
        }

        private void PopulateLoadingMessages(List<string> loadingMessages)
        {
            loadingMessages.Add("Rewinding tape...");
            loadingMessages.Add("Making something up...");
            //loadingMessages.Add("Asking {0} to help...");
            loadingMessages.Add("{0}, fine, I'm working on it...");
            loadingMessages.Add("Feeding the hamster...");
            loadingMessages.Add("Ok MingLee ™ Kappa");
            loadingMessages.Add("Typing ''sudo replay'' in console...");
            loadingMessages.Add("Searching for empty video tape...");
            loadingMessages.Add("Generating even better play...");
            loadingMessages.Add("Entering cheat codes...");
            loadingMessages.Add("NullReferenceException Kappa");
            loadingMessages.Add("Wololo");
            loadingMessages.Add("Increasing FPS to 144...");
            loadingMessages.Add("Loading...");
            loadingMessages.Add("Switching to quantum CPU...");
            //loadingMessages.Add("Can {0} do that instead?");
            loadingMessages.Add("Why do you like to torture me, {0}? FeelsBadMan");
            loadingMessages.Add("Upscaling to 4K");
        }
    }
}
