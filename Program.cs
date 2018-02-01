using System;
using System.Collections.Generic;
using com.tvd12.ezyfoxserver.client.entity;

namespace com.tvd12.ezyfoxserver.client
{

	class MainClass
	{
		public static void Main(string[] args)
		{
			var tmp = new Dictionary<String, Int32>();
			tmp["hello"] = 3005;
			var dict = new EzyObject();
			dict.putAll(tmp);
			dict.put("foo", "bar");
			var array = new EzyArray();
			array.add(true);
			array.add("i love you");
			array.add(dict);
			var value = array.get<bool>(0);
			Console.WriteLine("Hello World! " + array);

		}
	}
}
