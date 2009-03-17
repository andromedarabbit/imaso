#pragma once
#include <msclr/marshal.h>
#include "TimeSpan.h"

namespace msclr
{
	namespace interop
	{
		using namespace System;

		template <>
		inline TimeSpan marshal_as<TimeSpan, NTimeSpan>(const NTimeSpan& nativeObj) 
		{
			return TimeSpan(nativeObj.Ticks());
		}

		inline NTimeSpan marshal_as( TimeSpan% managedObj) 
		{
			return NTimeSpan(managedObj.Ticks);
		}


		template<>
		ref class context_node<NTimeSpan*, TimeSpan> : public context_node_base
		{
		private:
			NTimeSpan* toPtr;
			marshal_context context;

		public:
			context_node(NTimeSpan*& toObject, TimeSpan fromObject)
			{
				// Conversion logic starts here
				toPtr = NULL;
				toPtr = new NTimeSpan(fromObject.Ticks);
				toObject = toPtr;
			}

			~context_node()
			{
				this->!context_node();
			}

		protected:
			!context_node()
			{
				if (toPtr != NULL) {
				   delete toPtr;
				   toPtr = NULL;
				}
			}
		};
	}
}