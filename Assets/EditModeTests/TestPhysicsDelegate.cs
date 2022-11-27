using System;
using DWGames;
using DWGames.com.darkwing_games.core.Runtime.Util;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace EditModeTests
{


    public class TestPhysicsDelegate
    {

        private IPhysicsDelegate physicsDelegateMock;
        private Collider colliderMock;
        [SetUp]
        public void Setup()
        {
            physicsDelegateMock = Substitute.For<IPhysicsDelegate>();
            FooUsesPhysics.SetPhysicsDelegate(physicsDelegateMock);
            colliderMock = Substitute.For<Collider>();
        }

        [TearDown]
        public void TearDown()
        {
            FooUsesPhysics.SetPhysicsDelegate(new UnityPhysics());
        }
        
        
        [Test]
        public void shouldReturnTrue_whenMockReturnsNonEmptyArray()
        {
            
            //when
            physicsDelegateMock.OverlapSphere(Vector3.back, 3, -1).Returns(new Collider[]{colliderMock});
            
            //then
            var useOverlapSphere = FooUsesPhysics.UseOverlapSphere(Vector3.back, 3, -1);
            Assert.IsTrue(useOverlapSphere, "Should have non-empty array from physics.");
        }

        [Test]
        public void shouldReturnFalse_whenOverlapSphereGivesEmptyArray()
        {
            //when
            physicsDelegateMock.OverlapSphere(Vector3.back, 3, -1).Returns(Array.Empty<Collider>());
            
            //then
            var useOverlapSphere = FooUsesPhysics.UseOverlapSphere(Vector3.back, 3, -1);
            Assert.IsFalse(useOverlapSphere, "Should have empty array from physics.");
        }
        
        
    }
    
}