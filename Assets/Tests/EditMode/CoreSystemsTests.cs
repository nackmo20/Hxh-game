using System.IO;
using NUnit.Framework;

namespace HxHGame.Tests
{
    public sealed class CoreSystemsTests
    {
        string root;
        [SetUp] public void Setup(){root=Path.Combine(Path.GetTempPath(),"hxh-tests-"+System.Guid.NewGuid());}
        [TearDown] public void Teardown(){if(Directory.Exists(root))Directory.Delete(root,true);}

        [Test] public void SaveRoundTripPreservesProgression()
        {
            var system=new SaveSystem(root); var data=new SaveData{health=17,aura=9}; data.inventory.Add("test_item");
            system.Save(1,data); var loaded=system.Load(1);
            Assert.That(loaded.health,Is.EqualTo(17)); Assert.That(loaded.inventory,Contains.Item("test_item"));
        }
        [Test] public void ProfileSignalsAreDeterministic()
        {
            var p=new TraitProfile(); p.Observe("caution",2); p.Observe("caution"); Assert.That(p.caution,Is.EqualTo(3));
        }
        [Test] public void CombatSupportsNonDamageResolution()
        {
            var c=new StrategicCombat(new TraitProfile()); c.Act(2); c.Act(4); Assert.That(c.finished,Is.True); Assert.That(c.enemyHealth,Is.EqualTo(24));
        }
        [Test] public void GuardReducesIncomingDamage()
        {
            var c=new StrategicCombat(new TraitProfile()); int before=c.playerHealth; c.Act(1); Assert.That(before-c.playerHealth,Is.LessThanOrEqualTo(4));
        }
    }
}

