import searchFunction from '../components/IndexSearch.js';

const appOption = {
    components: {
        'index-select-type': indexSelectTypeOption,
        'index-banner': indexBannerOption
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

        // 使用 ref 來存儲異步加載的模塊
        const Login = ref(null);
        const Email = ref(null);
        const PasswordHash = ref(null);

        const rEmail = ref(null);
        const rPasswordHash = ref(null);
        const rLastName = ref(null);
        const rName = ref(null);
        const rPhone = ref(null);
        const Register = ref(null);

        onMounted(async () => {
            const { default: loginModule } = await import('../components/Login.js');
            const { default: registerModule } = await import('../components/Register.js');

            Login.value = () => loginModule.Login(Email, PasswordHash);

            rEmail.value = registerModule.Email.value;
            rPasswordHash.value = registerModule.PasswordHash.value;
            rLastName.value = registerModule.LastName.value;
            rName.value = registerModule.Name.value;
            rPhone.value = registerModule.Phone.value;
            Register.value = registerModule.Register;

            await indexSelectMoreOnMounted();
        });

        return {
            fnChangeText,
            selectTypes,
            search,
            Login,
            Email,
            PasswordHash,

            rEmail,
            rPasswordHash,
            rLastName,
            rName,
            rPhone,
            Register
        }
    }
};

const app = createApp(appOption);

app.mount("#app");