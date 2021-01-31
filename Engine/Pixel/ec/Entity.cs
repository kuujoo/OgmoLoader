﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace kuujoo.Pixel
{
    public class Entity
    {
        public Transform Transform { get; private set; }
        public bool Enabled => _enabled;
        public string Name { get; set; }
        public int Tag { get; set; }
        public ComponentList Components { get; private set; }
        public Scene Scene { get; set; }
        bool _enabled = true;
        public Entity()
        {
            Components = new ComponentList();
            Transform = new Transform();
        }
        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }
        public T AddComponent<T>(T component) where T: Component
        {
            component.Entity = this;
            Components.Add(component);
            component.Initialize();
            component.AddedToEntity();
            return component;
        }
        public T GetComponent<T>() where T : class
        {
            return Components.GetItemOfType<T>();
        }
        public List<T> GetComponents<T>() where T : class
        {
            return Components.GetItemsOfType<T>();
        }
        public ICoroutine StartCoroutine(IEnumerator enumerator)
        {
            return Engine.Instance.StartCoroutine(enumerator);
        }
    }
}
