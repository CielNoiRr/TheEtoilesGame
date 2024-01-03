// using System;
// using System.Collections.Generic;
// using MEC;
// using UnityEngine;

// namespace Everhood;

// public abstract class ShootProjectile__ : EventCommand
// {
// 	public delegate void ProjectileInfo(GameObject gameObject, int corridor);

// 	public static float localProjectilSpeedModifier = 1f;

// 	public static readonly Dictionary<Tuple<GameObject, GameObject>, GameObject> projectilesWithTarget = new Dictionary<Tuple<GameObject, GameObject>, GameObject>();

// 	public static readonly HashSet<GameObject> projectiles = new HashSet<GameObject>();

// 	public static readonly HashSet<GameObject> unjumpablesProjectiles = new HashSet<GameObject>();

// 	[SerializeField]
// 	private GameObject target;

// 	[SerializeField]
// 	private CollisionEvent collisionEvent;

// 	[SerializeField]
// 	private VirtualPathData virtualPathData;

// 	[SerializeField]
// 	private GameObject projectilPrefab;

// 	[SerializeField]
// 	private float minDistance = 0.2f;

// 	[Space(5f)]
// 	[SerializeField]
// 	[Range(0f, 100f)]
// 	private float speed;

// 	[SerializeField]
// 	private bool sinusMovement;

// 	[Space(5f)]
// 	[SerializeField]
// 	[Range(0f, 100f)]
// 	private float waveHeightX;

// 	[SerializeField]
// 	[Range(0f, 100f)]
// 	private float waveSpeedX;

// 	[Space(5f)]
// 	[SerializeField]
// 	[Range(0f, 100f)]
// 	private float waveHeightY;

// 	[SerializeField]
// 	[Range(0f, 100f)]
// 	private float waveSpeedY;

// 	[SerializeField]
// 	private bool customStartTimeSinWave;

// 	[SerializeField]
// 	private float startTimeSinWaveX;

// 	[SerializeField]
// 	private float startTimeSinWaveY;

// 	[Space(10f)]
// 	[SerializeField]
// 	private int projectilePoolSize = 20;

// 	[Space(10f)]
// 	[SerializeField]
// 	private bool debug;

// 	public ProjectileInfo projectileInfo;

// 	public VirtualPathData VirtualPathData => virtualPathData;

// 	public GameObject ProjectilPrefab => projectilPrefab;

// 	public int ProjectilePoolSize => projectilePoolSize;

// 	public abstract int GetCorridor();

// 	public virtual Vector3 GetProjectileStartPosition(int corridor)
// 	{
// 		return virtualPathData.GetVirtualPath().StartsPoints[corridor];
// 	}

// 	public void SetParameters(CollisionEvent collisionEvent, VirtualPathData virtualPathData, GameObject projectilPrefab, float speed)
// 	{
// 		this.collisionEvent = collisionEvent;
// 		this.virtualPathData = virtualPathData;
// 		this.projectilPrefab = projectilPrefab;
// 		this.speed = speed;
// 	}

// 	public void SetParameters(CollisionEvent collisionEvent, VirtualPathData virtualPathData, GameObject projectilPrefab, float speed, float waveHeightX, float waveSpeedX, float waveHeightY, float waveSpeedY)
// 	{
// 		this.collisionEvent = collisionEvent;
// 		this.virtualPathData = virtualPathData;
// 		this.projectilPrefab = projectilPrefab;
// 		this.speed = speed;
// 		sinusMovement = true;
// 		this.waveHeightX = waveHeightX;
// 		this.waveSpeedX = waveSpeedX;
// 		this.waveHeightY = waveHeightY;
// 		this.waveSpeedY = waveSpeedY;
// 	}

// 	public virtual GameObject GetTarget()
// 	{
// 		return target;
// 	}

// 	private void Awake()
// 	{
// 		localProjectilSpeedModifier = 1f;
// 		CreateProjectilPool();
// 		AddToCache();
// 	}

// 	private void CreateProjectilPool()
// 	{
// 		PoolManager.instance.CreatePool(projectilPrefab, projectilePoolSize);
// 	}

// 	private void AddToCache()
// 	{
// 		for (int i = 0; i < projectilePoolSize; i++)
// 		{
// 			GameObject gameObject = PoolManager.instance.ReuseObject(projectilPrefab, Vector3.zero, Quaternion.identity);
// 			if (!CollisionEvent.targetedCollisionEventCache.ContainsKey(new Tuple<GameObject, GameObject>(gameObject, target)))
// 			{
// 				collisionEvent.AddToCache(gameObject, target);
// 				projectilesWithTarget.Add(new Tuple<GameObject, GameObject>(gameObject, target), projectilPrefab);
// 			}
// 			if ((bool)(collisionEvent as DamagePlayerCollisionEvent))
// 			{
// 				DamagePlayerCollisionEvent damagePlayerCollisionEvent = (DamagePlayerCollisionEvent)collisionEvent;
// 				if (!damagePlayerCollisionEvent.IsJumpable)
// 				{
// 					if (!damagePlayerCollisionEvent.OverrideDeflectVisualAsJumpable)
// 					{
// 						unjumpablesProjectiles.Add(gameObject);
// 					}
// 				}
// 				else if (damagePlayerCollisionEvent.OverrideDeflectVisualAsNotJumpable)
// 				{
// 					unjumpablesProjectiles.Add(gameObject);
// 				}
// 			}
// 			if ((bool)(collisionEvent as BatchedCollisionEvent))
// 			{
// 				BatchedCollisionEvent batchedCollisionEvent = (BatchedCollisionEvent)collisionEvent;
// 				for (int j = 0; j < batchedCollisionEvent.CollisionEvents.Count; j++)
// 				{
// 					if (!(batchedCollisionEvent.CollisionEvents[j] as DamagePlayerCollisionEvent))
// 					{
// 						continue;
// 					}
// 					DamagePlayerCollisionEvent damagePlayerCollisionEvent2 = (DamagePlayerCollisionEvent)batchedCollisionEvent.CollisionEvents[j];
// 					if (!damagePlayerCollisionEvent2.IsJumpable)
// 					{
// 						if (!damagePlayerCollisionEvent2.OverrideDeflectVisualAsJumpable)
// 						{
// 							unjumpablesProjectiles.Add(gameObject);
// 						}
// 					}
// 					else if (damagePlayerCollisionEvent2.OverrideDeflectVisualAsNotJumpable)
// 					{
// 						unjumpablesProjectiles.Add(gameObject);
// 					}
// 				}
// 			}
// 			projectiles.Add(gameObject);
// 			gameObject.SetActive(value: false);
// 		}
// 	}

// 	public List<GameObject> GetProjectiles()
// 	{
// 		List<GameObject> list = new List<GameObject>();
// 		for (int i = 0; i < projectilePoolSize; i++)
// 		{
// 			GameObject gameObject = PoolManager.instance.ReuseObject(projectilPrefab, Vector3.zero, Quaternion.identity);
// 			gameObject.SetActive(value: false);
// 			list.Add(gameObject);
// 		}
// 		return list;
// 	}

// 	public override void Execute()
// 	{
// 		if (base.enabled)
// 		{
// 			int corridor = GetCorridor();
// 			GameObject projectile = PoolManager.instance.ReuseObject(projectilPrefab, virtualPathData.GetVirtualPath().StartsPoints[corridor], Quaternion.identity);
// 			Vector3 projectileStartPosition = GetProjectileStartPosition(corridor);
// 			Vector3 to = virtualPathData.GetVirtualPath().EndPoints[corridor];
// 			Timing.RunCoroutine(MoveProjectil(projectile, projectileStartPosition, to, corridor), MEC.Segment.FixedUpdate);
// 		}
// 	}

// 	public void SetCollisionEvent(CollisionEvent collisionEvent)
// 	{
// 		this.collisionEvent = collisionEvent;
// 	}

// 	public void SetTarget(GameObject target)
// 	{
// 		this.target = target;
// 	}

// 	public void SetPoolSize(int value)
// 	{
// 		projectilePoolSize = value;
// 	}

// 	public void SetSpeed(float value)
// 	{
// 		speed = value;
// 	}

// 	public virtual IEnumerator<float> MoveProjectil(GameObject projectile, Vector3 from, Vector3 to, int corridor)
// 	{
// 		Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
// 		projectile.transform.LookAt(to);
// 		projectile.transform.position = from;
// 		Vector3 direction = (to - projectile.transform.position).normalized;
// 		bool positive = direction.z > 0f;
// 		float time = Time.time;
// 		while (NotReachedTarget(direction.z, positive) && projectile != null && projectile.activeInHierarchy)
// 		{
// 			direction = (to - projectile.transform.position).normalized;
// 			float num = (customStartTimeSinWave ? (Time.time - (time - startTimeSinWaveX)) : Time.time);
// 			float num2 = (customStartTimeSinWave ? (Time.time - (time - startTimeSinWaveY)) : Time.time);
// 			float num3 = (sinusMovement ? (waveHeightX * Mathf.Sin(num * waveSpeedX)) : 0f);
// 			float num4 = (sinusMovement ? (waveHeightY * Mathf.Sin(num2 * waveSpeedY)) : 0f);
// 			Vector3 vector = new Vector3(direction.x + num3, direction.y + num4, direction.z);
// 			projectileRigidbody.MovePosition(projectile.transform.position + vector * (speed * localProjectilSpeedModifier) * Time.deltaTime);
// 			if (debug)
// 			{
// 				MonoBehaviour.print("DISTANCE : " + Vector3.Distance(projectile.transform.position, to));
// 				MonoBehaviour.print("projectile : " + projectile.transform.position);
// 				MonoBehaviour.print("to : " + to);
// 			}
// 			yield return float.NegativeInfinity;
// 		}
// 		if (projectile != null && projectile.activeInHierarchy)
// 		{
// 			projectileInfo?.Invoke(projectile, corridor);
// 			projectile.SetActive(value: false);
// 		}
// 	}

// 	private bool NotReachedTarget(float zDirection, bool positive)
// 	{
// 		if (positive)
// 		{
// 			return zDirection > 0f;
// 		}
// 		return zDirection < 0f;
// 	}

// 	public void OnDrawGizmosSelected()
// 	{
// 		virtualPathData.GetVirtualPath();
// 		for (int i = 0; i < virtualPathData.GetVirtualPath().StartsPoints.Length; i++)
// 		{
// 			Gizmos.color = Color.red;
// 			Gizmos.DrawWireSphere(virtualPathData.GetVirtualPath().StartsPoints[i], 1f);
// 			Gizmos.DrawWireSphere(virtualPathData.GetVirtualPath().EndPoints[i], 1f);
// 			Gizmos.DrawLine(virtualPathData.GetVirtualPath().StartsPoints[i], virtualPathData.GetVirtualPath().EndPoints[i]);
// 		}
// 	}

// 	private void OnDestroy()
// 	{
// 		localProjectilSpeedModifier = 1f;
// 	}
// }