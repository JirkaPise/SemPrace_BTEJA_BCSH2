function f(x: int): void{
	log(x);
	x = x + 1;
	if(x < 5)
	{
		f(x);
	}
}

f(0);