using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoidBehavior{
	public enum EdgeBehavior{
		NONE = 0,WRAP =1,BOUNCE =2
	}
	public class Boid : MonoBehaviour {
		private float adjust = 0.15f;
		public float GetAdjust {
			get{ 
				return adjust;}
		}
//		private float minForce = 3.0f;
		private float _maxForce= 6.0f;
//		private float minSpeed = 6.0f;
		private float _maxSpeed= 12.0f;

		private float _distance ;
		private float _drawScale ;
		private float _maxForceSQ;
		private float _maxSpeedSQ;
		private Vector3 _velocity = Vector3.zero;
		private Vector3 _position = Vector3.zero;
		private Vector3 _oldPosition = Vector3.zero;
		private Vector3 _acceleration = Vector3.zero;
		private Vector3 _steeringForce = Vector3.zero;
//		private Vector2 _screenCoords;
//		private var _renderData : DisplayObject;
		private EdgeBehavior _edgeBehavior = EdgeBehavior.BOUNCE;
		private float _boundsRadius = 6f;
		private Vector3 _boundsCentre = Vector3.zero;
		private float _radius  = 0f;
		private float _wanderTheta  = 0.0f;
		private float _wanderPhi  = 0.0f;
		private float _wanderPsi  = 0.0f;
		private float _wanderRadius  = 16.0f;
		private float _wanderDistance  = 60.0f;//60
		private float _wanderStep  = 0.25f;
		private bool _lookAtTarget  = true;

		private Transform _target;
		public MeshRenderer sphereRT;
		public void Initial(float maxForce, float maxSpeed,Transform target){
			setProperties ();
			_maxForce *= adjust;
			_maxSpeed *= adjust;

			_target = target;
			//_maxForce = maxForce;
			//_maxSpeed = maxSpeed;
		}
		public void SetAdjust(float value){
			adjust = value;
			setProperties ();
		}
		private void setProperties()
		{
			float minForce = 3.0f * adjust;
			float maxForce = 6.0f * adjust;
			float minSpeed = 8.0f * adjust;//6
			float maxSpeed = 12.0f * adjust;
			float minWanderDistance = 10.0f * adjust;
			float maxWanderDistance = 100.0f * adjust;
			float minWanderRadius = 5.0f * adjust;
			float maxWanderRadius = 20.0f * adjust;
			float minWanderStep = 0.1f * adjust;
			float maxWanderStep = 0.9f * adjust;

//			minWanderDistance = 10;
//			maxWanderDistance = 10;
//			minWanderRadius = 50f;
//			maxWanderRadius = 50f;
			_maxForce = Random.Range(minForce,maxForce);
			_maxSpeed = Random.Range(minSpeed,maxSpeed);
			_wanderDistance = Random.Range(minWanderDistance, maxWanderDistance);
			_wanderRadius = Random.Range(minWanderRadius, maxWanderRadius);
			_wanderStep = Random.Range(minWanderStep,maxWanderStep);
			_boundsRadius = 6;;
//			_boundsCentre = transform.parent.position;
			transform.localPosition = new Vector3 (Random.Range (-1* _boundsRadius * 0.5f, _boundsRadius * 0.5f), Random.Range (-1* _boundsRadius * 0.5f, _boundsRadius * 0.5f),Random.Range (-1* _boundsRadius * 0.5f, _boundsRadius * 0.5f));

			Vector3 vel  = new Vector3(Random.Range(-2, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f));
			_velocity += vel;
		}
		public void ChangeColor(bool show){
//			sphereRT.enabled = show;	
		}

		public void BoidUpdate () {
			_oldPosition = _position;

			_velocity+=_acceleration;

			if ( _velocity.sqrMagnitude > _maxSpeedSQ )
			{
				_velocity.Normalize();
				_velocity*=_maxSpeed;
			}

			_position+=_velocity;
			_acceleration = Vector3.zero;

			if ( _edgeBehavior == EdgeBehavior.NONE  )
			{
				return;
			}

//			if( _position != _oldPosition )
//			{
//				
//				float distance = Vector3.Distance(_position, _boundsCentre);
//
//				if( distance > _boundsRadius + _radius )
//				{
////					Debug.Log ("Hit Edge");
//					switch( _edgeBehavior )
//					{
//					case EdgeBehavior.BOUNCE :
//
//						_position-=_boundsCentre;
//						_position.Normalize();
//						_position *=(_boundsRadius + _radius);
//						_velocity*=-1;
//						_position+=_velocity;
//						_position+=_boundsCentre;
//
//						break;
//					}
//				}
//			}

			transform.LookAt (_target);
			transform.localPosition = _position;
		}
	
		public void seek( Vector3 target ,float  multiplier )
		{
			_steeringForce = Steer(target);

			if ( multiplier != 1.0 )
			{
				_steeringForce*=multiplier;
			}
			_acceleration+=_steeringForce;
		}


		private Vector3 Steer( Vector3 target, bool ease = false, float easeDistance =100 ){
			_steeringForce = target;
			_steeringForce -=_position;

//			_distance = _steeringForce.normalized();
			_distance = 0.2f;
			if ( _distance > 0.00001 )
			{
				if ( _distance < easeDistance && ease )
				{
					_steeringForce *= (_maxSpeed * ( _distance / easeDistance ));
				}
				else
				{
					_steeringForce*=(_maxSpeed);
				}
				_steeringForce -= _velocity;

				if ( _steeringForce.sqrMagnitude > _maxForceSQ )
				{
					_steeringForce.Normalize();
					_steeringForce*=_maxForce;
				}
			}

			return _steeringForce;
		}

		public void wander( float multiplier = 1.0f ) 
		{
			_wanderTheta += -_wanderStep + Random.value * _wanderStep * 2;
			_wanderPhi += -_wanderStep + Random.value * _wanderStep * 2;
			_wanderPsi += -_wanderStep + Random.value * _wanderStep * 2;

			if ( Random.value < 0.5 )
			{
				_wanderTheta = -_wanderTheta;
			}

			Vector3 pos = _velocity;

			pos.Normalize();
			pos*=_wanderDistance;
			pos+=_position;

			Vector3 offset = new Vector3(_wanderRadius * Mathf.Cos(_wanderTheta),_wanderRadius * Mathf.Sin(_wanderPhi), _wanderRadius * Mathf.Cos(_wanderPsi));
		
			_steeringForce = Steer(pos+offset);

			if ( multiplier != 1.0 )
			{
				_steeringForce*=multiplier;
			}

			_acceleration+=_steeringForce;
		}

		public void flock( List<Boid> boids , float separationWeight = 0.5f , float alignmentWeight  = 0.1f, float cohesionWeight = 0.2f, float separationDistance = 100.0f,float alignmentDistance = 200.0f, float cohesionDistance = 200.0f )
		{
			separate(boids, separationDistance, separationWeight);
			align(boids, alignmentDistance, alignmentWeight);
			cohesion(boids, cohesionDistance, cohesionWeight);
		}

		public void flee( Vector3 target , float panicDistance = 100f, float multiplier = 1.0f )
		{
			_distance = Vector3.Distance(_position, target);

			if ( _distance > panicDistance )
			{
				return;
			}

			_steeringForce = Steer(target, true, -panicDistance);

			if ( multiplier != 1.0 )
			{
				_steeringForce*=multiplier;
			}

			_steeringForce *= -1;
			_acceleration+=_steeringForce;
		}

		public void separate(List<Boid> boids, float separationDistance = 50.0f, float multiplier = 1.0f)
		{
			_steeringForce = getSeparation(boids, separationDistance);

			if ( multiplier != 1.0 )
			{
				_steeringForce*=multiplier;
			}

			_acceleration+=_steeringForce;
		}

		public void align(List<Boid> boids, float neighborDistance = 40.0f,float multiplier = 1.0f )
		{
			_steeringForce = getAlignment(boids, neighborDistance);

			if ( multiplier != 1.0 )
			{
				_steeringForce*=multiplier;
			}

			_acceleration+=_steeringForce;
		}
		public void cohesion( List<Boid> boids,float neighborDistance = 10.0f,float multiplier = 1.0f )
		{
			_steeringForce = getCohesion(boids, neighborDistance);

			if ( multiplier != 1.0 )
			{
				_steeringForce*=multiplier;
			}

			_acceleration+=_steeringForce;
		}

		private Vector3 getSeparation(List<Boid> boids,float separation = 25.0f ) 
		{
			Vector3 force = Vector3.zero;
			Vector3 difference = Vector3.zero;
			float distance;
			int count = 0;
			Boid boid;

			for (int i = 0;i < boids.Count; i++) 
			{
				boid = boids[i];

				distance = Vector3.Distance(_position, boid.transform.localPosition);

				if ( distance > 0 && distance < separation )
				{
					difference = _position-boid.transform.localPosition;
					difference.Normalize();
					difference*=(1 / distance);

					force+=difference;
					count++;
				}
			}

			if ( count > 0 )
			{
				force*=(1 / count);
			}

			return force;
		}

		private Vector3 getAlignment( List<Boid> boids, float neighborDistance = 50.0f )
		{
			Vector3 force= Vector3.zero;
			float distance ;
			int count = 0;
			Boid boid;

			for (int i = 0;i < boids.Count; i++) 
			{
				boid = boids[i];
				distance = Vector3.Distance(_position, boid.transform.localPosition);

				if ( distance > 0 && distance < neighborDistance )
				{
					force+=(boid._velocity);
					count++;
				}
			}

			if ( count > 0 )
			{
				force*=(1 / count);

				if ( force.sqrMagnitude > _maxForceSQ )
				{
					force.Normalize();
					force*=_maxForce;
				}
			}

			return force;
		}
		private Vector3 getCohesion( List<Boid> boids, float neighborDistance  = 50.0f ) 
		{
			Vector3 force = Vector3.zero;
			float distance;
			int count = 0;
			Boid boid;

			for (int i = 0;i < boids.Count; i++) 
			{
				boid = boids[i];
				distance = Vector3.Distance(_position, boid.transform.localPosition);

				if ( distance > 0 && distance < neighborDistance )
				{
					force+=(boid.transform.localPosition);
					count++;
				}
			}

			if ( count > 0 )
			{
				force*=(1 / count);
				force = Steer(force);

				return force;
			}

			return force;
		}

	}

}