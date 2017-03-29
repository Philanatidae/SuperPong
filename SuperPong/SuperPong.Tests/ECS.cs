using System;
using ECS;
using ECS.Exceptions;
using NUnit.Framework;

namespace SuperPong.Tests
{
	[TestFixture]
	public class ECS
	{
		
		[Test]
		public void EntityCreation()
		{
			Engine engine = new Engine();

			Entity entity = engine.CreateEntity();
			Assert.NotNull(entity);
		}

		[Test]
		public void EntityComponents()
		{
			Engine engine = new Engine();
			Entity entity = engine.CreateEntity();

			{
				ECSComponent1 comp = new ECSComponent1();
				entity.AddComponent(comp);
				Assert.True(entity.HasComponent<ECSComponent1>());

				ECSComponent1 backComp = entity.GetComponent<ECSComponent1>();
				Assert.NotNull(backComp);
				Assert.AreSame(comp, backComp);

				entity.RemoveComponent<ECSComponent1>();
				Assert.False(entity.HasComponent<ECSComponent1>());
				Assert.NotNull(comp);
			}

			Assert.Catch(typeof(ComponentNotFoundException),
                        entity.RemoveComponent<ECSComponent1>);
			
			{
				entity.AddComponent(new ECSComponent1());
				Assert.Catch(typeof(ComponentAlreadyExistsException), () =>
				{
					entity.AddComponent(new ECSComponent1());
				});
				entity.RemoveComponent<ECSComponent1>();
			}

			Assert.DoesNotThrow(() => {
				entity.GetComponent<ECSComponent1>();
			});
			Assert.IsNull(entity.GetComponent<ECSComponent1>());
		}

		[Test]
		public void Families()
		{
			Engine engine = new Engine();

			Entity entity = engine.CreateEntity();

			entity.AddComponent(new ECSComponent1());
			Assert.True(Family.All(typeof(ECSComponent1)).Get().Matches(entity));
			Assert.False(Family.All(typeof(ECSComponent1), typeof(ECSComponent2)).Get().Matches(entity));
			Assert.True(Family.All(typeof(ECSComponent1)).Exclude(typeof(ECSComponent2)).Get().Matches(entity));
			Assert.True(Family.All(typeof(ECSComponent1)).Exclude(typeof(ECSComponent2), typeof(ECSComponent3)).Get().Matches(entity));
			Assert.True(Family.One(typeof(ECSComponent1)).Get().Matches(entity));
			Assert.False(Family.Exclude(typeof(ECSComponent1)).Get().Matches(entity));
			Assert.True(Family.Exclude(typeof(ECSComponent2)).Get().Matches(entity));

			entity.AddComponent(new ECSComponent2());
			Assert.True(Family.All(typeof(ECSComponent1)).Get().Matches(entity));
			Assert.True(Family.All(typeof(ECSComponent1), typeof(ECSComponent2)).Get().Matches(entity));
			Assert.False(Family.All(typeof(ECSComponent1), typeof(ECSComponent2), typeof(ECSComponent3)).Get().Matches(entity));
			Assert.True(Family.One(typeof(ECSComponent1)).Get().Matches(entity));
			Assert.True(Family.One(typeof(ECSComponent1), typeof(ECSComponent2)).Get().Matches(entity));
			Assert.True(Family.One(typeof(ECSComponent1), typeof(ECSComponent2), typeof(ECSComponent3)).Get().Matches(entity));
			Assert.False(Family.One(typeof(ECSComponent1)).Exclude(typeof(ECSComponent2)).Get().Matches(entity));

			{
				Family family1 = Family.All(typeof(ECSComponent1)).One(typeof(ECSComponent2)).Exclude(typeof(ECSComponent3)).Get();
				Family family2 = Family.All(typeof(ECSComponent1)).One(typeof(ECSComponent2)).Exclude(typeof(ECSComponent3)).Get();

				Assert.AreEqual(family1, family2);
			}
		}

		[Test]
		public void EngineQueries()
		{
			Engine engine = new Engine();

			Entity entity = engine.CreateEntity();

			ImmutableList<Entity> entities = engine.GetEntities();
			Assert.NotNull(entities);
			Assert.AreEqual(entities.Count, 1);

			{
				ImmutableList<Entity> entities2 = engine.GetEntities();
				Assert.AreSame(entities, entities2);
			}

			ImmutableList<Entity> ecs1Entities = engine.GetEntitiesFor(Family.All(typeof(ECSComponent1)).Get());
			Assert.NotNull(ecs1Entities);
			Assert.AreEqual(ecs1Entities.Count, 0);

			{
				ImmutableList<Entity> ecs1Entities2 = engine.GetEntitiesFor(Family.All(typeof(ECSComponent1)).Get());
				Assert.AreSame(ecs1Entities, ecs1Entities2);
			}

			entity.AddComponent(new ECSComponent1());
			Assert.AreEqual(ecs1Entities.Count, 1);

			entity.RemoveComponent<ECSComponent1>();
			Assert.AreEqual(ecs1Entities.Count, 0);
		}
	}

	public class ECSComponent1 : IComponent { }
	public class ECSComponent2 : IComponent { }
	public class ECSComponent3 : IComponent { }
}
