using System;
using System.Collections.Generic;
using System.Text;

namespace MetaDataSample
{
    class BirdPlayer
    {
        private const string assemblyName = "MetaDataSample.exe";
        private const string keyName = "Bird";
        private AbstractBird bird = null;

        public void PlayNext()
        {
            MoveNext();
            AbstractBird.Save(bird, assemblyName);
            bird.Sing();
        }

        public AbstractBird MoveNext()
        {
            if (bird == null) { bird = AbstractBird.Load(); }
            else if (bird is Dove) { bird = new Duck(); }
            else { bird = new Dove(); }
            return bird;
        }
    }
}
