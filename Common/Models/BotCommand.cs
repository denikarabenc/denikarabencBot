using Common.Helpers;
using System;
using System.Xml.Serialization;

namespace Common.Models
{
    [Serializable]
    public class BotCommand
    {
        private bool isTimed;
        private bool useAppendedStrings;
        private bool isActive;
        private string command;        
        private string message;
        private CommandType type;
        private UserType userPermission;

        public BotCommand()
        { }

        public BotCommand(string command, string message) : this(command, message, UserType.Regular)
        {
        }
        public BotCommand(string command, string message, CommandType type) : this(command, message, UserType.Regular, command.Contains("{0}"), false, true, type)
        {
        }

        public BotCommand(string command, string message, UserType userPermission) : this(command, message, userPermission, command.Contains("{0}"))
        {
        }

        public BotCommand(string command, string message, UserType userPermission, bool useAppendedStrings) : this(command, message, userPermission, useAppendedStrings, false)
        {
        }

        public BotCommand(string command, string message, UserType userPermission, bool useAppendedStrings, bool isTimed) : this(command, message, userPermission, useAppendedStrings, isTimed, true, CommandType.ReadCommand)
        {
        }

        public BotCommand(string command, string message, UserType userPermission, bool useAppendedStrings, bool isTimed, bool isActive) : this(command, message, userPermission, useAppendedStrings, isTimed, isActive, CommandType.ReadCommand)
        {
        }

        public BotCommand(string command, string message, UserType userPermission, bool useAppendedStrings, bool isTimed, bool isActive, CommandType type)
        {
            command.ThrowIfNull(nameof(command));
            message.ThrowIfNull(nameof(message));
            this.command = command;
            this.message = message;
            this.type = type;
            this.isTimed = isTimed;
            this.useAppendedStrings = useAppendedStrings;
            this.userPermission = userPermission;
            this.isActive = isActive;
        }

        [XmlElement("IsTimedElementName")]
        public bool IsTimed { get => isTimed; set => isTimed = value; }
        
        [XmlElement("UseAppendedStringsElementName")]
        public bool UseAppendedStrings { get => useAppendedStrings; set => useAppendedStrings = value; }
        [XmlElement("CommandElementName")]
        public string Command { get => command; set => command = value; }
        [XmlElement("MessageElementName")]
        public string Message { get => message; set => message = value; }
        [XmlElement("TypeElementName")]
        public CommandType Type { get => type; set => type = value; }
        [XmlElement("UserPermissionElementName")]
        public UserType UserPermission { get => userPermission; set => userPermission = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
    }
}
