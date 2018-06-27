using Common.Commands;
using Common.Helpers;
using Common.Interfaces;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Timers;
using TwitchBot.BotCommands;

namespace TwitchBot
{
    public class TimedCommandHandler : IDisposable
    {
        private double intervalCommandIsSent; //in minutes
        private int commandCounter;
        private readonly IIrcClient irc;
        private readonly BotCommandsRepository botCommandsRepository;

        private Timer timer;

        public TimedCommandHandler(BotCommandsRepository botCommandsRepository, IIrcClient irc)
        {
            irc.ThrowIfNull(nameof(irc));
            botCommandsRepository.ThrowIfNull(nameof(botCommandsRepository));
            this.irc = irc;
            this.botCommandsRepository = botCommandsRepository;

            intervalCommandIsSent = 40 * 1000 * 60;

            timer = new Timer(IntervalCommandIsSent);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<BotCommand> botCommands = botCommandsRepository.GetTimedCommands(); //So if new command is added, it can be automatically updated here.
            if (botCommands.Count > 0)
            {
                if (commandCounter < botCommands.Count)
                {
                    irc.SendChatMessage(botCommands[commandCounter].Message);
                    commandCounter++;
                    if (commandCounter == botCommands.Count)
                        commandCounter = 0;
                }
                else
                {
                    commandCounter = 0;
                }
            }
        }

        public double IntervalCommandIsSent
        {
            get
            {
                return intervalCommandIsSent;
            }
            set
            {
                intervalCommandIsSent = value * 1000 * 60;
                timer.Stop();                
                if (intervalCommandIsSent == 0)
                {
                    timer.AutoReset = false;
                    timer.Enabled = false;
                }
                else
                {
                    timer.Interval = intervalCommandIsSent;
                    timer.AutoReset = true;
                    timer.Enabled = true;
                }
            }
        }

        public void Dispose()
        {
            timer.Stop();
            timer.Dispose();
        }
    }
}
