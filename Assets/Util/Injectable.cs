using System;

public interface IInjectable<T>
{
	void Inject(T instance);
}

public interface IInjectable<T1, T2>
{
	void Inject(T1 ins1, T2 ins2);
}

public interface IInjectable<T1, T2, T3>
{

	void Inject(T1 ins1, T2 ins2, T3 ins3);

}
