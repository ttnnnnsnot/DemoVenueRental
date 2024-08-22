import searchFunction from '../components/IndexSearch.js';
import registerOption from '../components/Register.js';

const appOption = {
    components: {
        'index-select-type': indexSelectTypeOption,
        'index-banner': indexBannerOption,
        'register': registerOption
    },
    setup() {

        const fetchData1 = () => fetchWithParams('def/sport');
        const fetchData2 = () => fetchWithParams('def/area');

        const {
            selectTypes,
            fnChangeText,
            onMounted: indexSelectMoreOnMounted
        } = indexSelectMore([fetchData1, fetchData2]);

        const search = () => searchFunction.search(selectTypes);

        const registerComponent = ref(null);
        const showRegisterModal = () => {
            console.log(registerComponent.value);
            if (registerComponent.value) {
                registerComponent.value.showModel();
            }
        };

        const Login = ref(null);
        const Email = ref(null);
        const PasswordHash = ref(null);
     
        onMounted(async () => {
            const { default: loginModule } = await import('../components/Login.js');
            Login.value = () => loginModule.Login(Email, PasswordHash);
            await indexSelectMoreOnMounted();
        });

        return {
            fnChangeText,
            selectTypes,
            search,
            Login,
            Email,
            PasswordHash,
            showRegisterModal,
            registerComponent
        }
    }
};

const app = createApp(appOption);

app.mount("#app");