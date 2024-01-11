using System;
using UnityEngine;

namespace Logic
{
	public class Singleton<T> where T : class, new()
	{
		private static T instance = null;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new T();
				}

				return instance;
			}
		}
		public virtual void Init()
		{

		}

		public virtual void Destroy()
		{
			instance = null;
		}
	}
}

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
	// http://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/
	// null 체크 비용이 생각보다 비싸기 때문에 cache 해서 사용
	private static bool _hasInstance;

	private static T _instance;

	protected bool Used;

	public static T Instance
	{
		get
		{
			if (_hasInstance)
			{
			}
			else
			{
				FindOrCreateInstance(true);
			}

			return _instance;
		}
	}

	protected void Awake()
	{
		CheckDuplication();

		if (Used && CheckDontDestroyOnLoad())
		{
			DontDestroyOnLoad(gameObject);
		}

		OnAwake();
	}

	protected void OnDestroy()
	{
		if (Used)
		{
			_hasInstance = false;
			_instance = null;
		}

		OnDestroyed();
	}

	protected virtual bool CheckDontDestroyOnLoad()
	{
		return false;
	}

	protected virtual void OnAwake()
	{
	}

	protected virtual void OnDestroyed()
	{
	}

	private void CheckDuplication()
	{
		MonoSingleton<T> original = this;

		T[] objects = FindObjectsOfType<T>();
		int count = objects.Length;
		if (count >= 2)
		{
			for (int i = 0; i < count; ++i)
			{
				var comp = objects[i] as MonoSingleton<T>;
				Debug.Assert(comp != null, "comp != null");

				if (comp.Used)
				{
					original = comp;
					break;
				}
				if (this != comp)
				{
					Destroy(comp.gameObject);
				}
			}
		}

		if (original != this)
		{
			Destroy(gameObject);
		}

		if (_instance == null)
		{
			_instance = original as T;
			_hasInstance = _instance != null;
			original.Used = true;
		}
	}


	protected static void SetInstance(T instance)
	{
		_instance = instance;
		_hasInstance = _instance != null;

		var singletone = _instance as MonoSingleton<T>;
		Debug.Assert(singletone != null, "singletone != null");

		singletone.Used = true;
	}
	private static void FindOrCreateInstance(bool showError)
	{
		T[] objects = FindObjectsOfType<T>();
		int count = objects.Length;
		if (count >= 1)
		{
			SetInstance(objects[0]);
		}
		else
		{
			Create(typeof(T).ToString());
		}
	}

	public static T Create(string name)
	{
		if (_hasInstance)
		{
			return _instance;
		}

		var obj = new GameObject(name);
		DontDestroyOnLoad(obj);

		// 실행중이 아닌데 에디터에서 접근 하는 경우
		if (Application.isPlaying == false)
		{
			obj.hideFlags = HideFlags.HideAndDontSave;
		}

		var comp = obj.AddComponent<T>();
		SetInstance(comp);
		return comp;
	}

	public static bool HasInstance()
	{
		return _hasInstance;
	}
}
