#include <improbable/worker.h>
#include <cstdint>
#include <iostream>
#include <string>
#include <chrono>
#include <thread>
#include <improbable/general/WorldTransform.h>

using ::improbable::general::WorldTransform;
using namespace worker;

static const std::string kWorkerType = "CppWorker";
static const std::string kLoggerName = "CppWorker.cc";

int main(int argc, char** argv) {
  if (argc < 2 || 3 < argc) {
    return argc;
  }

  worker::ConnectionParameters parameters;
  parameters.WorkerType = kWorkerType;
  parameters.WorkerId = argv[1];
  parameters.Network.ConnectionType = worker::NetworkConnectionType::kTcp;
  parameters.Network.UseExternalIp = false;

  std::string hostname = "localhost";
  if (argc == 3) {
    hostname = argv[2];
  }

  worker::Connection connection = Connection::ConnectAsync(hostname, 7777, parameters).Get();
  connection.SendInterestedComponents<WorldTransform>(0);
  worker::View view;

  worker::Dispatcher dispatcher = worker::Dispatcher();
 
  std::cout << kWorkerType << " started" << std::endl;
  for (;;) {
	  // Get the operations since the last time.
	  auto op_list = connection.GetOpList(0);
	  // Process and dispatch.
	  view.Process(op_list);
	  //UpdateEntity(connection, view, 2);
  }
}