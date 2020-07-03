﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;

namespace NonPersistentObjectsDemo.Module.BusinessObjects {

    class DemoDataCreator {
        private IObjectSpace ObjectSpace;
        public DemoDataCreator(IObjectSpace objectSpace) {
            this.ObjectSpace = objectSpace;
        }
        public void CreateDemoObjects() {
            if(ObjectSpace.CanInstantiate(typeof(Project))) {
                if(!ObjectSpace.CanInstantiate(typeof(Technology))) {
                    var typesInfo = ObjectSpace.TypesInfo;
                    var npos = new NonPersistentObjectSpace(typesInfo, ((DevExpress.ExpressApp.DC.TypesInfo)typesInfo).FindEntityStore(typeof(DevExpress.ExpressApp.DC.NonPersistentTypeInfoSource)));
                    ((CompositeObjectSpace)ObjectSpace).AdditionalObjectSpaces.Add(npos);
                    ((CompositeObjectSpace)ObjectSpace).AutoCommitAdditionalObjectSpaces = true;
                    new NPTechnologyAdapter(npos);
                }
                CreateProjects();
                CreateEpochs();
            }
        }
        private void CreateProjects() {
            var p1 = CreateProject("Project X", "B");
            var p2 = CreateProject("Project Y", "B");
            p2.Features.Add(new Feature() { Name = "Feature 1", Progress = 3.5 });
            p2.Features.Add(new Feature() { Name = "Feature 2", Progress = 0 });
            p2.Features.Add(new Feature() { Name = "Feature 3", Progress = 1 });
            p2.Resources.Add(new Resource() { Name = "Resource A", URI = "a", Embed = true });
            p2.Resources.Add(new Resource() { Name = "Resource B", URI = "b", Priority = 2 });
            p2.Resources.Add(new Resource() { Name = "Resource C", URI = "c", Priority = 1 });
            var p3 = CreateProject("Project Z", "AAA");
            var p4 = CreateProject("Project Unknown", "XHD");
        }
        private Project CreateProject(string name, string group) {
            var project = ObjectSpace.CreateObject<Project>();
            project.CodeName = name;
            project.Group = new Group() { Name = group };
            return project;
        }
        private void CreateEpochs() {
            var t1 = CreateTechnology("Tech 1", "Technology 1");
            var t2 = CreateTechnology("Tech 2", "Technology 2");
            var t3 = CreateTechnology("Tech 3", "Technology 3");
            var e1 = CreateEpoch("Stone Age");
            var e2 = CreateEpoch("Nowadays");
            var e3 = CreateEpoch("Future");
            e2.Agents.Add(new Agent() { Name = "Agent X", Progress = 80 });
            e2.Agents.Add(new Agent() { Name = "Agent Orange", Progress = 0 });
            e2.Technologies.Add(t1);
            e2.Technologies.Add(t2);
            e2.Technologies.Add(t3);
        }
        private Epoch CreateEpoch(string name) {
            var obj = ObjectSpace.CreateObject<Epoch>();
            obj.Name = name;
            return obj;
        }
        private Technology CreateTechnology(string name, string description) {
            var tech = ObjectSpace.CreateObject<Technology>();
            tech.Name = name;
            tech.Description = description;
            return tech;
        }
    }
}
