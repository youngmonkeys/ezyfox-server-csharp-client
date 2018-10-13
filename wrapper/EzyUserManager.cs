using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;
         
namespace com.tvd12.ezyfoxserver.client.wrapper
{
	public class EzyUserManager
	{
		protected EzyUser me;
		protected IDictionary<long, EzyUser> usersById = new Dictionary<long, EzyUser>();
		protected IDictionary<String, EzyUser> usersByName = new Dictionary<String, EzyUser>();

		public EzyUser getMe()
		{
			return me;
		}

		public EzyUser getUser(long userId)
		{
			if (usersById.ContainsKey(userId))
			{
				return usersById[userId];
			}
			return null;
		}

		public EzyUser getUser(String username)
		{
			if (usersByName.ContainsKey(username))
			{
				return usersByName[username];
			}
			return null;
		}

		public void addUser(EzyUser user)
		{
			usersById[user.getId()] = user;
			usersByName[user.getName()] = user;
			if (user.isMe())
			{
				this.me = user;
			}
		}

		public void removeUser(EzyUser user)
		{
			usersById.Remove(user.getId());
			usersByName.Remove(user.getName());
		}

		public void removeUser(long userId)
		{
			var user = getUser(userId);
			if (user != null)
			{
				removeUser(user);
			}
		}

		public void removeUser(String userName)
		{
			var user = getUser(userName);
			if (user != null)
			{
				removeUser(user);
			}
		}

		public void addUsers(ICollection<EzyUser> users)
		{
			foreach(var user in users)
			{
				addUser(user);
			}
		}
	}
}
