using System;

namespace LuaFramework
{
    public class Message : IMessage
    {
        /// <summary>
        /// The name of the notification instance 
        /// </summary>
        protected string m_Name;

        /// <summary>
        /// The type of the notification instance
        /// </summary>
        protected string m_Type;

        /// <summary>
        /// The body of the notification instance
        /// </summary>
        protected object m_Body;

        public Message(string name)
            : this(name, null, null)
        {
        }

        public Message(string name, object body)
            : this(name, body, null)
        {
        }

        public Message(string name, object body, string type)
        {
            m_Name = name;
            m_Body = body;
            m_Type = type;
        }

        /// <summary>
        /// Get the string representation of the <c>Notification instance</c>
        /// </summary>
        /// <returns>The string representation of the <c>Notification</c> instance</returns>
        public override string ToString()
        {
            string msg = "Notification Name: " + this.name;
            msg += "\nBody:" + ((this.body == null) ? "null" : this.body.ToString());
            msg += "\nType:" + ((this.type == null) ? "null" : this.type);
            return msg;
        }

        /// <summary>
        /// The name of the <c>Notification</c> instance
        /// </summary>
        public virtual string name
        {
            get { return m_Name; }
        }

        /// <summary>
        /// The body of the <c>Notification</c> instance
        /// </summary>
        /// <remarks>This accessor is thread safe</remarks>
        public virtual object body
        {
            get
            {
                // Setting and getting of reference types is atomic, no need to lock here
                return m_Body;
            }
            set
            {
                // Setting and getting of reference types is atomic, no need to lock here
                m_Body = value;
            }
        }

        /// <summary>
        /// The type of the <c>Notification</c> instance
        /// </summary>
        /// <remarks>This accessor is thread safe</remarks>
        public virtual string type
        {
            get
            {
                // Setting and getting of reference types is atomic, no need to lock here
                return m_Type;
            }
            set
            {
                // Setting and getting of reference types is atomic, no need to lock here
                m_Type = value;
            }
        }
    }
}
