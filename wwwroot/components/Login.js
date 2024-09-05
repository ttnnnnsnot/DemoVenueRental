const loginModel = () => {
    const performLoggedIn = inject('loggedIn');
    const email = ref('');
    const passwordHash = ref('');
    const errorMsg = ref('');

    const clear = () => {
        email.value = '';
        passwordHash.value = '';
        errorMsg.value = '';
    }

    const showModel = async () => {
        clear();
        $("#loginModal").modal("show");
        $.validator.unobtrusive.parse($("#loginModal"));
    }

    const login = async () => {
        if (!$("#loginModalForm").valid())
            return;

        const formData = new FormData(document.getElementById('loginModalForm'));

        const data = Object.fromEntries(
            Array.from(formData.entries()).map(([key, value]) => [key.replace('Login.', ''), value])
        );

        try {
            const results = await API.POST("User/Login", data);

            if (results && results.state) {
                clear();
                $("#loginModal").modal("hide");
                performLoggedIn();
            } else {
                errorMsg.value = results.message;
                passwordHash.value = '';
            }

        } catch (error) {
            errorMsg.value = error.message;
            passwordHash.value = '';
        }
    };

    return {
        email,
        passwordHash,
        login,
        showModel,
        errorMsg
    };
}

const loginOption = defineAsyncComponent(async () => {
    return {
        template: await API.GetTemplate("/Template/_Login"),
        setup() {
            const {
                email,
                passwordHash,
                login,
                showModel,
                errorMsg
            } = loginModel();

            return {
                email,
                passwordHash,
                login,
                showModel,
                errorMsg
            };
        }
    }
});

export default loginOption