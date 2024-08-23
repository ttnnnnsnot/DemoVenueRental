import registerOption from '../components/Register.js';
import loginOption from '../components/Login.js';
import headerOption from '../components/Header.js';

export const useRegister = () => {

    const {
        isLoggedIn,
        currentState,
        headerLinks,
        setShowType,
        onMounted
    } = headerOption();

    const registerComponent = ref(null);
    const showRegisterModal = () => {
        if (registerComponent.value) {
            registerComponent.value.showModel();
        }
    };


    const loginComponent = ref(null);
    const showLoginModal = () => {
        if (loginComponent.value) {
            loginComponent.value.showModel();
        }
    };

    return {
        isLoggedIn,
        currentState,
        headerLinks,
        setShowType,
        onMounted,

        registerComponent,
        showRegisterModal,

        loginComponent,
        showLoginModal
    };
}

export const layoutOption = {
    components: {
        'register-module': registerOption,
        'login-module': loginOption
    }
}