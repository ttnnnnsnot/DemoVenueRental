import registerOption from '../components/Register.js';
import loginOption from '../components/Login.js';
import headerTemplate from '../components/Header.js';

const useRegister = () => {

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

    const isLoggedIn = ref(null);

    const Logouted = async () => {
        await Logout();
        isLoggedIn.value = await IsLoggedIn();
    }

    const LoggedIn = async () => {
        isLoggedIn.value = await IsLoggedIn();
    }

    const onBeforeMount = async () => {
        if (!isEmptyObject(accessDenied)) {
            Alert.addDanger("您沒有權限!")
        }
    };

    const onMounted = async () => {
        await nextTick();
        isLoggedIn.value = await IsLoggedIn();
    };

    return {
        registerComponent,
        showRegisterModal,

        loginComponent,
        showLoginModal,

        isLoggedIn,
        Logouted,
        LoggedIn,

        onBeforeMount,
        onMounted
    };
}

export const setupLayout = () => {
    // Layout.js
    const { registerComponent, showRegisterModal,
        loginComponent, showLoginModal,
        isLoggedIn,
        Logouted,
        LoggedIn,

        onBeforeMount: LayoutonBeforeMount,
        onMounted: LayoutonMounted } = useRegister();

    const headerCurrentState = ref(1);

    watch(loginComponent, (newVal) => {
        if (!isEmptyObject(noLogin)) {
            showLoginModal();
        }
    });

    provide('isLoggedIn', isLoggedIn);
    provide('currentState', headerCurrentState);

    return {
        showRegisterModal, registerComponent,
        loginComponent, showLoginModal,
        LoggedIn, Logouted,

        LayoutonBeforeMount,
        LayoutonMounted,

        headerCurrentState
    };
}


export const layoutOption = {
    components: {
        'register-module': registerOption,
        'login-module': loginOption,
        'header-module': headerTemplate
    }
}