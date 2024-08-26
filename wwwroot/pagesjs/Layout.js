import registerOption from '../components/Register.js';
import loginOption from '../components/Login.js';
import headerTemplate from '../components/Header.js';

export const useRegister = () => {

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

    const Logout = async () => {
        try {
            return await API.GET('User/Logout');
        } catch (error) {
            console.log(error);
            return false;
        }
    }

    return {
        registerComponent,
        showRegisterModal,

        loginComponent,
        showLoginModal,

        Logout
    };
}

export const layoutOption = {
    components: {
        'register-module': registerOption,
        'login-module': loginOption,
        'header-module': headerTemplate
    }
}