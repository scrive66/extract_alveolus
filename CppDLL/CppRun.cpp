#include <mist/filter/median.h>

extern "C"
{
	__declspec(dllexport) bool extract_alveolus(void* pCT, void* pMK, int size1, int size2, int size3, 
		double reso1, double reso2, double reso3);
}

bool extract_alveolus(void* pCT, void* pMK, int size1, int size2, int size3,
	double reso1, double reso2, double reso3)
{
	using namespace mist;
	// �f�[�^�̔z����擾����.
	auto ct = reinterpret_cast<short *>(pCT);
	auto mk = reinterpret_cast<short *>(pMK);

	// �������l�������s��.
	median(ct, mk, 3, 3, 3);


	return(true);
}
