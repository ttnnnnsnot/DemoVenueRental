const loginModel = (props) => {
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

        const data = {
            Email: email.value,
            PasswordHash: passwordHash.value
        };

        try {
            const results = await API.POST("User/Login", data);

            if (results && results.state) {
                clear();
                $("#loginModal").modal("hide");
                props.onLoggedIn();
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
        props: {
            onLoggedIn: {
                type: Function,
                required: true
            }
        },
        setup(props) {
            const {
                email,
                passwordHash,
                login,
                showModel,
                errorMsg
            } = loginModel(props);

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