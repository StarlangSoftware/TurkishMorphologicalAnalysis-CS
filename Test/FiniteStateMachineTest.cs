using System.Collections.Generic;
using DataStructure;
using MorphologicalAnalysis;
using NUnit.Framework;

namespace Test
{
    public class FiniteStateMachineTest
    {
        FiniteStateMachine fsm;
        List<State> stateList;

        [SetUp]
        public void Setup()
        {
            fsm = new FiniteStateMachine("turkish_finite_state_machine.xml");
            stateList = fsm.GetStates();
        }

        [Test]
        public void TestStateCount()
        {
            Assert.AreEqual(141, stateList.Count);
        }
        
        [Test]
        public void TestStartEndStates()
        {
            var endStateCount = 0;
            foreach (var state in stateList){
                if (state.IsEndState())
                {
                    endStateCount++;
                }
            }
            Assert.AreEqual(37, endStateCount);
            var posCounts = new CounterHashMap<string>();
            foreach (var state in stateList){
                if (state.GetPos() != null)
                {
                    posCounts.Put(state.GetPos());
                }
            }
            Assert.AreEqual(1, posCounts["HEAD"]);
            Assert.AreEqual(6, posCounts["PRON"]);
            Assert.AreEqual(1, posCounts["PROP"]);
            Assert.AreEqual(8, posCounts["NUM"]);
            Assert.AreEqual(7, posCounts["ADJ"]);
            Assert.AreEqual(1, posCounts["INTERJ"]);
            Assert.AreEqual(1, posCounts["DET"]);
            Assert.AreEqual(1, posCounts["ADVERB"]);
            Assert.AreEqual(1, posCounts["QUES"]);
            Assert.AreEqual(1, posCounts["CONJ"]);
            Assert.AreEqual(26, posCounts["VERB"]);
            Assert.AreEqual(1, posCounts["POSTP"]);
            Assert.AreEqual(1, posCounts["DUP"]);
            Assert.AreEqual(11, posCounts["NOUN"]);
        }

        [Test]

        public void TestTransitionCount()
        {
            var transitionCount = 0;
            foreach (var state in stateList){
                transitionCount += fsm.GetTransitions(state).Count;
            }
            Assert.AreEqual(779, transitionCount);
        }

        [Test]

        public void TestTransitionWith()
        {
            var transitionCounts = new CounterHashMap<string>();
            foreach (var state in stateList){
                var transitions = fsm.GetTransitions(state);
                foreach (var transition in transitions){
                    transitionCounts.Put(transition.ToString());
                }
            }
            var topList = transitionCounts.TopN(5);
            Assert.AreEqual("0", topList[0].Key);
            Assert.AreEqual(111, topList[0].Value);
            Assert.AreEqual("lAr", topList[1].Key);
            Assert.AreEqual(37, topList[1].Value);
            Assert.AreEqual("DHr", topList[2].Key);
            Assert.AreEqual(28, topList[2].Value);
            Assert.AreEqual("Hn", topList[3].Key);
            Assert.AreEqual(24, topList[3].Value);
            Assert.AreEqual("lArH", topList[4].Key);
            Assert.AreEqual(23, topList[4].Value);
        }

        [Test]

        public void TestTransitionWithName()
        {
            var transitionCounts = new CounterHashMap<string>();
            foreach (var state in stateList){
                var transitions = fsm.GetTransitions(state);
                foreach (var transition in transitions){
                    if (transition.With() != null)
                    {
                        transitionCounts.Put(transition.With());
                    }
                }
            }
            var topList = transitionCounts.TopN(4);
            Assert.AreEqual("^DB+VERB+CAUS", topList[0].Key);
            Assert.AreEqual(33, topList[0].Value);
            Assert.AreEqual("^DB+VERB+PASS", topList[1].Key);
            Assert.AreEqual(31, topList[1].Value);
            Assert.AreEqual("A3PL", topList[2].Key);
            Assert.AreEqual(28, topList[2].Value);
            Assert.AreEqual("LOC", topList[3].Key);
            Assert.AreEqual(24, topList[3].Value);
        }
    }
}