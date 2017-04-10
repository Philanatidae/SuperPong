using System;
using ECS;
using Events;
using SuperPong.Components;
using SuperPong.Events;

namespace SuperPong.Systems
{
	public class LivesSystem : EntitySystem, IEventListener
	{
		readonly Family _livesFamily = Family.All(typeof(LivesComponent), typeof(FontComponent)).Get();

		readonly ImmutableList<Entity> _livesEntities;

		public LivesSystem(Engine engine):base(engine)
		{
			_livesEntities = engine.GetEntitiesFor(_livesFamily);
		}

		public override void Update(float dt)
		{
			foreach (Entity entity in _livesEntities)
			{
				LivesComponent livesComp = entity.GetComponent<LivesComponent>();
				FontComponent fontComp = entity.GetComponent<FontComponent>();

				fontComp.Content = livesComp.Lives.ToString();
			}
		}

		void DecreaseLifeCount(Player player)
		{
			foreach (Entity livesEntity in _livesEntities)
			{
				LivesComponent livesComp = livesEntity.GetComponent<LivesComponent>();

				if (livesComp.For == player)
				{
					livesComp.Lives--;
					break;
				}
			}
		}

		public void RegisterEventListeners()
		{
			EventManager.Instance.RegisterListener<GoalEvent>(this);
		}

		public void UnregisterEventListeners()
		{
			EventManager.Instance.UnregisterListener(this);
		}

		public bool Handle(IEvent evt)
		{
			if (evt is GoalEvent)
			{
				GoalEvent goalEvent = evt as GoalEvent;
				DecreaseLifeCount(goalEvent.Goal.GetComponent<GoalComponent>().For);
			}

			return false;
		}
	}
}
