extends Node

var local_user_id : String = ""

func _init() -> void:
	var init_opts = EOS.Platform.InitializeOptions.new()
	init_opts.product_name = EOSCredentials.PRODUCT_NAME
	init_opts.product_version = EOSCredentials.PRODUCT_ID
	
	var init_results := EOS.Platform.PlatformInterface.initialize(init_opts)
	if not EOS.is_success(init_results):
		print("Failed to initialize EOS SDK: ", EOS.result_str(init_results))
		return
	print("Iniatalized EOS SDK")
	
	var create_opts = EOS.Platform.CreateOptions.new()
	create_opts.product_id = EOSCredentials.PRODUCT_ID
	create_opts.sandbox_id = EOSCredentials.SANDBOX_ID
	create_opts.deployment_id = EOSCredentials.DEPLOYMENT_ID
	create_opts.client_id = EOSCredentials.P2P_PLAYER_CLIENT_ID
	create_opts.client_secret = EOSCredentials.P2P_PLAYER_CLIENT_SECRET
	create_opts.encryption_key = EOSCredentials.ENCRYPTION_KEY
	
	if OS.get_name() == "Windows":
		create_opts.flags = EOS.Platform.PlatformFlags.DisableOverlay
		
	var create_success := EOS.Platform.PlatformInterface.create(create_opts)
	if not create_success:
		print("Failed to create EOS Platform")
		return
	print("Created EOS Platform")
	
	print("ESO SDK v " + EOS.Version.VersionInterface.get_version())
	
	IEOS.logging_interface_callback.connect(_on_logging_interface)
	var res := EOS.Logging.set_log_level(EOS.Logging.LogCategory.AllCategories, EOS.Logging.LogLevel.Info)
	if not EOS.is_success(res):
		print("Failed to set log level: ", EOS.result_str(res))
		return
	print("Setup EOS Logging success")
	
	IEOS.connect_interface_login_callback.connect(_on_connect_login)

func _on_logging_interface(msg):
	msg = EOS.Logging.LogMessage.from(msg)
	print("[EOS] %s | %s" % [msg.category, msg.message])
	
func _on_connect_login(data: Dictionary) -> void:
	if not data.success:
		print("Login failed")
		EOS.print_result(data)
		return
	print_rich("[b]Login successfull[/b]: local_user_id=", data.local_user_id)
	local_user_id = data.local_user_id
	HAuth.product_user_id = local_user_id

func device_id_login():
	var credentials := EOS.Auth.Credentials.new()
	credentials.type = EOS.Auth.LoginCredentialType.DeviceCode
	credentials.id = "0" # Use device id??
	credentials.token = "0" #Figure this out later
	
	var login_opts := EOS.Auth.LoginOptions.new()
	login_opts.credentials = credentials
	
	pass
	
