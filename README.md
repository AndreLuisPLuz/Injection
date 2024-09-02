# Intended usage:
CONFIG
	[Injector(<Scope>)]
	protected <Class> injectClass() => return new <Class>();

	[InheritanceInjector(<Scope>)]
	protected <AbstractClass> injectClass() => return new <ConcreteClass>();

INJECTION
	[Inject]
	private <Class> object;